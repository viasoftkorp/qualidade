package services

import (
	"strconv"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/enums"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/shopspring/decimal"
)

type InspecaoSaidaSagaService struct {
	interfaces.IInspecaoSaidaSagaService
	InspecaoSaidaRepository             interfaces.IInspecaoSaidaRepository
	InspecaoSaidaExecutadoWebRepository interfaces.IInspecaoSaidaExecutadoWebRepository
	InspecaoSaidaSagaService            interfaces.IExternalInspecaoSaidaSagaService
	OrdemProducaoRepository             interfaces.IOrdemProducaoRepository
	LocaisRepository                    interfaces.ILocaisRepository
	ParametroRepository                 interfaces.IParametroRepository
	EstoqueLocalRepository              interfaces.IEstoquePedidoVendaRepository
	ProdutoRepository                   interfaces.IProdutoRepository
	Uow                                 unit_of_work.UnitOfWork
}

func NewInspecaoSaidaSagaService(
	inspecaoSaidaRepository interfaces.IInspecaoSaidaRepository,
	inspecaoSaidaExecutadoWebRepository interfaces.IInspecaoSaidaExecutadoWebRepository,
	inspecaoSaidaSagaService interfaces.IExternalInspecaoSaidaSagaService,
	ordemProducaoRepository interfaces.IOrdemProducaoRepository,
	locaisRepository interfaces.ILocaisRepository,
	parametroRepository interfaces.IParametroRepository,
	estoqueLocalRepository interfaces.IEstoquePedidoVendaRepository,
	produtoRepository interfaces.IProdutoRepository, uow unit_of_work.UnitOfWork) interfaces.IInspecaoSaidaSagaService {
	return &InspecaoSaidaSagaService{
		InspecaoSaidaRepository:             inspecaoSaidaRepository,
		InspecaoSaidaExecutadoWebRepository: inspecaoSaidaExecutadoWebRepository,
		InspecaoSaidaSagaService:            inspecaoSaidaSagaService,
		OrdemProducaoRepository:             ordemProducaoRepository,
		LocaisRepository:                    locaisRepository,
		ParametroRepository:                 parametroRepository,
		EstoqueLocalRepository:              estoqueLocalRepository,
		ProdutoRepository:                   produtoRepository,
		Uow:                                 uow,
	}
}

func (service *InspecaoSaidaSagaService) RemoverSaga(id string) error {
	err := service.InspecaoSaidaSagaService.RemoverSaga(id)
	if err != nil {
		return err
	}

	err = service.InspecaoSaidaExecutadoWebRepository.RemoverSaga(id)
	if err != nil {
		return err
	}
	return nil
}

func (service *InspecaoSaidaSagaService) ReprocessarSaga(id string) error {
	err := service.InspecaoSaidaSagaService.ReprocessarSaga(id)
	if err != nil {
		return err
	}
	return nil
}

func (service *InspecaoSaidaSagaService) BuscarSagas(baseFilters *models.BaseFilter, filters *dto.ProcessamentoInspecaoSaidaFilters, estorno bool) (*dto.GetAllProcessamentoInspecaoSaidaOutput, error) {
	var output dto.GetAllProcessamentoInspecaoSaidaOutput
	result, err := service.InspecaoSaidaSagaService.BuscarSagas(baseFilters, filters, estorno)
	if err != nil {
		return nil, err
	}

	localDescricaoMapping := make(map[int]string)

	output.TotalCount = result.TotalCount
	for _, item := range result.Items {
		descricaoProduto, err := service.ProdutoRepository.BuscarProdutoDescricao(item.CodigoProduto)
		if err != nil {
			return nil, err
		}

		itemProcessamento := dto.ProcessamentoInspecaoSaidaOutput{
			IdSaga:              item.Id,
			Status:              item.Status,
			Erro:                item.Erro,
			NumeroRetentativas:  item.NumeroRetentativas,
			NumeroExecucoes:     item.NumeroExecucoes,
			QuantidadeTotal:     item.QuantidadeTotal,
			Resultado:           item.Resultado,
			CodigoProduto:       item.CodigoProduto,
			DescricaoProduto:    item.CodigoProduto + " - " + descricaoProduto,
			IdUsuarioExecucao:   item.IdUsuarioExecucao,
			NomeUsuarioExecucao: item.NomeUsuarioExecucao,
			DataExecucao:        item.DataExecucao,
			Lote:                item.Lote,
			Estorno:             item.Estorno,
			Transferencias:      make([]dto.ProcessamentoInspecaoSaidaTransferenciaOutput, 0),
			OdfRetrabalho:       nil,
		}

		if item.OrdemRetrabalho != nil {
			itemProcessamento.OdfRetrabalho = item.OrdemRetrabalho.OrdemFabricacao
		}

		for _, transferencia := range item.Transferencias {
			if itemProcessamento.Odf == 0 {
				odfPai, err := service.OrdemProducaoRepository.BuscarOrdemPaiHistoricoMovimentacao(item.Lote, item.CodigoProduto, transferencia.LocalOrigem)
				if err != nil {
					return nil, err
				} else if odfPai != nil {
					itemProcessamento.Odf = *odfPai
				} else {
					itemProcessamento.Odf = transferencia.OrdemFabricacao
				}
			}

			descricaoOrigem, containsLocal := localDescricaoMapping[transferencia.LocalOrigem]
			if !containsLocal {
				descricaoOrigem, err = service.LocaisRepository.BuscarLocalDescricao(transferencia.LocalOrigem)
				if err != nil {
					return nil, err
				}

				localDescricaoMapping[transferencia.LocalOrigem] = descricaoOrigem
			}

			descricaoDestino, containsLocal := localDescricaoMapping[transferencia.LocalDestino]
			if !containsLocal {
				descricaoDestino, err = service.LocaisRepository.BuscarLocalDescricao(transferencia.LocalDestino)
				if err != nil {
					return nil, err
				}

				localDescricaoMapping[transferencia.LocalDestino] = descricaoDestino
			}

			itemProcessamento.Transferencias = append(itemProcessamento.Transferencias, dto.ProcessamentoInspecaoSaidaTransferenciaOutput{
				OrdemFabricacao:       transferencia.OrdemFabricacao,
				NumeroPedido:          transferencia.NumeroPedido,
				Quantidade:            transferencia.Quantidade,
				LocalOrigem:           transferencia.LocalOrigem,
				DescricaoLocalOrigem:  strconv.Itoa(transferencia.LocalOrigem) + " - " + descricaoOrigem,
				LocalDestino:          transferencia.LocalDestino,
				DescricaoLocalDestino: strconv.Itoa(transferencia.LocalDestino) + " - " + descricaoDestino,
				TipoTransferencia:     transferencia.TipoTransferencia,
			})
		}

		output.Items = append(output.Items, itemProcessamento)
	}

	return &output, nil
}

func (service *InspecaoSaidaSagaService) PublicarSagaInspecaoSaidaEstorno(inspecaoSaidaRecno int) (bool, *dto.ValidacaoDTO, error) {
	inspecaoSaidaExecutadoWeb, err := service.InspecaoSaidaExecutadoWebRepository.BuscarInspecaoSaidaExecutadoWeb(inspecaoSaidaRecno)
	if err != nil {
		return false, nil, err
	}

	saga, err := service.InspecaoSaidaSagaService.BuscarSaga(inspecaoSaidaExecutadoWeb.IdInspecaoSaidaSaga)
	if err != nil {
		return false, nil, err
	}

	movimentarEstoqueInput := dto.MovimentarEstoqueInspecaoBackgroundInputDto{
		IdSaga:          utils.NewGuidAsString(),
		Lote:            saga.Lote,
		Estorno:         true,
		RecnoInspecao:   saga.RecnoInspecao,
		CodigoProduto:   saga.CodigoProduto,
		OrdemFabricacao: saga.OrdemFabricacao,
		OrdemRetrabalho: saga.OrdemRetrabalho,
	}

	movimentarEstoqueInput.Transferencias, err = service.PreencherTransferenciasEstorno(saga)
	if err != nil {
		return false, nil, err
	}

	err = service.PublicarSaga(movimentarEstoqueInput, saga.RecnoInspecao, true)
	if err != nil {
		return false, nil, err
	}

	return true, nil, nil
}

func (service *InspecaoSaidaSagaService) PreencherTransferenciasEstorno(saga *dto.SagaInspecaoSaidaOutput) ([]dto.InspecaoSaidaTransferenciaBackgroundInputDto, error) {
	transferencias := make([]dto.InspecaoSaidaTransferenciaBackgroundInputDto, 0)

	var pesoLiquido *float64
	var pesoBruto *float64

	for _, transferencia := range saga.Transferencias {
		// Locais invertidos pois e acao de estorno
		localOrigem := transferencia.LocalDestino
		localDestino := transferencia.LocalOrigem

		descricaoOrigem, err := service.LocaisRepository.BuscarLocalDescricao(localOrigem)
		if err != nil {
			return nil, err
		}

		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(localDestino)
		if err != nil {
			return nil, err
		}

		estoqueLocalValores, err := service.EstoqueLocalRepository.BuscarEstoqueLocalValoresPorProduto(saga.CodigoProduto, saga.Lote, transferencia.OrdemFabricacao, localOrigem)
		if err != nil {
			return nil, err
		}

		if estoqueLocalValores.PesoLiquido == nil {
			pesoLiquido = nil
		} else {
			pesoLiquidoFloat := utils.DecimalToFloat64(*estoqueLocalValores.PesoLiquido)
			pesoLiquido = &pesoLiquidoFloat
		}

		if estoqueLocalValores.PesoBruto == nil {
			pesoBruto = nil
		} else {
			pesoBrutoFloat := utils.DecimalToFloat64(*estoqueLocalValores.PesoBruto)
			pesoBruto = &pesoBrutoFloat
		}

		series := make([]dto.InspecaoSaidaSerieProducaoBackgroundInputDto, 0)
		for _, serie := range transferencia.SeriesProducao {
			series = append(series, dto.InspecaoSaidaSerieProducaoBackgroundInputDto{
				Serie:      serie.Serie,
				RecnoSerie: serie.RecnoSerie,
			})
		}
		transferencias = append(transferencias, dto.InspecaoSaidaTransferenciaBackgroundInputDto{
			Quantidade:        transferencia.Quantidade,
			LocalOrigem:       localOrigem,
			LocalDestino:      localDestino,
			Documento:         service.GetDocumento(descricaoOrigem, descricaoDestino),
			Fator:             1,
			NumeroPedido:      transferencia.NumeroPedido,
			TipoTransferencia: transferencia.TipoTransferencia,
			PesoLiquido:       pesoLiquido,
			PesoBruto:         pesoBruto,
			DataValidade:      utils.StringToTime(estoqueLocalValores.DataValidade),
			DataFabricacao:    utils.StringToTime(estoqueLocalValores.DataFabricacao),
			UltimoValorPago:   utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
			OrdemFabricacao:   transferencia.OrdemFabricacao,
			SeriesProducao:    series,
			Sequencial:        transferencia.Sequencial,
		})
	}

	return transferencias, nil
}

func (service *InspecaoSaidaSagaService) PublicarSagaInspecaoSaida(input *dto.FinalizarInspecaoInput) (bool, *dto.ValidacaoDTO, error) {
	localOrigem, err := service.LocaisRepository.BuscarLocalSaida()
	if err != nil {
		return false, nil, err
	}

	descricaoOrigem, err := service.LocaisRepository.BuscarLocalDescricao(localOrigem)
	if err != nil {
		return false, nil, err
	}

	inspecaoSaida, err := service.InspecaoSaidaRepository.BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao)
	if err != nil {
		return false, nil, err
	}

	ordemProducao, estoqueLocalValores, err := service.GetOdfEstoqueLocal(inspecaoSaida.Odf, localOrigem)
	if err != nil {
		return false, nil, err
	}

	movimentarEstoqueInput := service.GetMovimentarEstoqueInput(inspecaoSaida.Recno, ordemProducao, false)

	movimentarEstoqueInput.Transferencias, err = service.PreencherTransferenciasFromInput(input, ordemProducao, estoqueLocalValores, localOrigem, descricaoOrigem)
	if err != nil {
		return false, nil, err
	}

	movimentarEstoqueInput.Resultado = service.GetResultado(inspecaoSaida.QtdLote, movimentarEstoqueInput.Transferencias)
	if input.Rnc != nil {
		movimentarEstoqueInput.OrdemRetrabalho = &dto.InspecaoSaidaOrdemRetrabalhoBackgroundDto{
			Quantidade:            input.QuantidadeRetrabalhada,
			CodigoProduto:         movimentarEstoqueInput.CodigoProduto,
			CodigoCliente:         inspecaoSaida.Cliente,
			NumeroPedido:          inspecaoSaida.Pedido,
			OrdemFabricacaoOrigem: inspecaoSaida.Odf,
			Retrabalho:            true,
			Materias:              make([]dto.InspecaoSaidaOrdemRetrabalhoMaterialackgroundDto, 0),
			Maquinas:              make([]dto.InspecaoSaidaOrdemRetrabalhoMaquinaBackgroundDto, 0),
			IdRnc:                 *input.Rnc.IdRnc,
		}
		if ordemProducao.DataEntrega != "" {
			dataEntregaDate, _ := time.Parse("20060102", ordemProducao.DataEntrega)
			movimentarEstoqueInput.OrdemRetrabalho.DataEntrega = &dataEntregaDate
		}
		service.InspecaoSaidaRepository.PreencherInformacoesMaterialRetrabalho(*input.Rnc, movimentarEstoqueInput.OrdemRetrabalho)
		service.InspecaoSaidaRepository.PreencherInformacoesMaquinaRetrabalho(*input.Rnc, movimentarEstoqueInput.OrdemRetrabalho)
		movimentarEstoqueInput.IdRnc = input.Rnc.IdRnc
		movimentarEstoqueInput.CodigoRnc = input.Rnc.CodigoRnc
	}

	err = service.PublicarSaga(movimentarEstoqueInput, inspecaoSaida.Recno, false)
	if err != nil {
		return false, nil, err
	}

	return true, nil, nil
}

func (service *InspecaoSaidaSagaService) PublicarSaga(movimentarEstoqueInput dto.MovimentarEstoqueInspecaoBackgroundInputDto, inspecaoSaidaRecno int, estorno bool) error {
	_ = service.Uow.Begin()
	defer service.Uow.UnitOfWorkGuard()

	inspecaoExecutadoWeb, err := service.InspecaoSaidaExecutadoWebRepository.BuscarInspecaoSaidaExecutadoWeb(inspecaoSaidaRecno)
	if err != nil {
		_ = service.Uow.Rollback()
		return err
	} else if inspecaoExecutadoWeb == nil || inspecaoExecutadoWeb.Id == "" || inspecaoExecutadoWeb.Estorno != estorno {
		quantidadeTotalTransferida := 0.0
		for _, transferencia := range movimentarEstoqueInput.Transferencias {
			quantidadeTotalTransferida += transferencia.Quantidade
		}
		quantidadeTotalTransferidaDecimal := decimal.NewFromFloat(quantidadeTotalTransferida)
		inspecaoSaidaExecutadaWeb := &entities.InspecaoSaidaExecutadoWeb{
			RecnoInspecaoSaida:    inspecaoSaidaRecno,
			IdInspecaoSaidaSaga:   movimentarEstoqueInput.IdSaga,
			Estorno:               estorno,
			QuantidadeTransferida: &quantidadeTotalTransferidaDecimal,
			IdRnc:                 movimentarEstoqueInput.IdRnc,
			CodigoRnc:             movimentarEstoqueInput.CodigoRnc,
		}
		err = service.InspecaoSaidaExecutadoWebRepository.InserirInspecaoSaidaExecutadoWeb(inspecaoSaidaExecutadaWeb)
		if err != nil {
			_ = service.Uow.Rollback()
			return err
		}
	} else {
		movimentarEstoqueInput.IdSaga = inspecaoExecutadoWeb.IdInspecaoSaidaSaga
	}

	err = service.Uow.Complete()
	if err != nil {
		_ = service.Uow.Rollback()
		return err
	}

	_, err = service.InspecaoSaidaSagaService.PublicarSaga(movimentarEstoqueInput)
	if err != nil {
		return err
	}

	return nil
}

func (service *InspecaoSaidaSagaService) GetSeriesInput(series []models.Serie, numerosPacotesSeriesJaMovidos *[]string, quantidadeSaida float64) []dto.InspecaoSaidaSerieProducaoBackgroundInputDto {
	seriesInput := make([]dto.InspecaoSaidaSerieProducaoBackgroundInputDto, 0)

	totalAdicionados := 0.0
	for _, serie := range series {
		if totalAdicionados == quantidadeSaida {
			break
		}
		if utils.Contains(*numerosPacotesSeriesJaMovidos, serie.Serie) {
			continue
		}

		totalAdicionados += 1

		if totalAdicionados <= quantidadeSaida {
			seriesInput = append(seriesInput, dto.InspecaoSaidaSerieProducaoBackgroundInputDto{
				Serie:      serie.Serie,
				RecnoSerie: serie.RecnoSerie,
			})

			*numerosPacotesSeriesJaMovidos = append(*numerosPacotesSeriesJaMovidos, serie.Serie)
		}
	}

	return seriesInput
}

func (service *InspecaoSaidaSagaService) GetMovimentarEstoqueInput(inspecaoSaidaRecno int, ordemProducao *models.OrdemProducao, estorno bool) dto.MovimentarEstoqueInspecaoBackgroundInputDto {
	movimentarEstoqueInput := dto.MovimentarEstoqueInspecaoBackgroundInputDto{
		IdSaga:          utils.NewGuidAsString(),
		Lote:            ordemProducao.Lote,
		Estorno:         estorno,
		RecnoInspecao:   inspecaoSaidaRecno,
		CodigoProduto:   ordemProducao.CodigoProduto,
		OrdemFabricacao: ordemProducao.ODF,
	}

	return movimentarEstoqueInput
}

func (service *InspecaoSaidaSagaService) PreencherTransferenciasFromInspecaoSaida(inspecaoSaida *models.InspecaoSaidaJoin, ordemProducao *models.OrdemProducao, estoqueLocalValores *models.EstoqueLocalValores, localOrigem int, descricaoOrigem string) ([]dto.InspecaoSaidaTransferenciaBackgroundInputDto, error) {
	transferencias := make([]dto.InspecaoSaidaTransferenciaBackgroundInputDto, 0)
	numerosSeriesJaMovidos := make([]string, 0)

	quantidadeAprovada := utils.DecimalToFloat64(inspecaoSaida.QuantidadeAprovada)
	quantidadeReprovada := utils.DecimalToFloat64(inspecaoSaida.QuantidadeReprovada)
	quantidadeRetrabalhada := utils.DecimalToFloat64(inspecaoSaida.QuantidadeRetrabalhada)
	var pesoLiquido *float64
	var pesoBruto *float64

	if estoqueLocalValores.PesoLiquido == nil {
		pesoLiquido = nil
	} else {
		pesoLiquidoFloat := utils.DecimalToFloat64(*estoqueLocalValores.PesoLiquido)
		pesoLiquido = &pesoLiquidoFloat
	}

	if estoqueLocalValores.PesoBruto == nil {
		pesoBruto = nil
	} else {
		pesoBrutoFloat := utils.DecimalToFloat64(*estoqueLocalValores.PesoBruto)
		pesoBruto = &pesoBrutoFloat
	}

	series, err := service.EstoqueLocalRepository.BuscarSeries(estoqueLocalValores.Recno)
	if err != nil {
		return nil, err
	}

	if quantidadeAprovada > 0 {
		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(inspecaoSaida.CodigoLocalAprovado)
		if err != nil {
			return nil, err
		}

		seriesInput := service.GetSeriesInput(series, &numerosSeriesJaMovidos, quantidadeAprovada)
		transferencias = append(transferencias, dto.InspecaoSaidaTransferenciaBackgroundInputDto{
			Quantidade:        quantidadeAprovada,
			LocalOrigem:       localOrigem,
			LocalDestino:      inspecaoSaida.CodigoLocalAprovado,
			Documento:         service.GetDocumento(descricaoOrigem, descricaoDestino),
			Fator:             1,
			NumeroPedido:      ordemProducao.NumeroPedido,
			TipoTransferencia: enums.Aprovado,
			PesoLiquido:       pesoLiquido,
			PesoBruto:         pesoBruto,
			DataValidade:      utils.StringToTime(estoqueLocalValores.DataValidade),
			DataFabricacao:    utils.StringToTime(estoqueLocalValores.DataFabricacao),
			UltimoValorPago:   utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
			SeriesProducao:    seriesInput,
			OrdemFabricacao:   ordemProducao.ODF,
		})
	}

	if quantidadeReprovada > 0 {
		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(inspecaoSaida.CodigoLocalReprovado)
		if err != nil {
			return nil, err
		}

		seriesInput := service.GetSeriesInput(series, &numerosSeriesJaMovidos, quantidadeReprovada)
		transferencias = append(transferencias, dto.InspecaoSaidaTransferenciaBackgroundInputDto{
			Quantidade:        quantidadeReprovada,
			LocalOrigem:       localOrigem,
			LocalDestino:      inspecaoSaida.CodigoLocalReprovado,
			Documento:         service.GetDocumento(descricaoOrigem, descricaoDestino),
			Fator:             1,
			NumeroPedido:      ordemProducao.NumeroPedido,
			TipoTransferencia: enums.Reprovado,
			PesoLiquido:       pesoLiquido,
			PesoBruto:         pesoBruto,
			DataValidade:      utils.StringToTime(estoqueLocalValores.DataValidade),
			DataFabricacao:    utils.StringToTime(estoqueLocalValores.DataFabricacao),
			UltimoValorPago:   utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
			SeriesProducao:    seriesInput,
			OrdemFabricacao:   ordemProducao.ODF,
		})
	}

	if quantidadeRetrabalhada > 0 {
		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(inspecaoSaida.CodigoLocalRetrabalho)
		if err != nil {
			return nil, err
		}

		seriesInput := service.GetSeriesInput(series, &numerosSeriesJaMovidos, quantidadeRetrabalhada)
		transferencias = append(transferencias, dto.InspecaoSaidaTransferenciaBackgroundInputDto{
			Quantidade:        quantidadeRetrabalhada,
			LocalOrigem:       localOrigem,
			LocalDestino:      inspecaoSaida.CodigoLocalRetrabalho,
			Documento:         service.GetDocumento(descricaoOrigem, descricaoDestino),
			Fator:             1,
			NumeroPedido:      ordemProducao.NumeroPedido,
			TipoTransferencia: enums.Retrabalhado,
			PesoLiquido:       pesoLiquido,
			PesoBruto:         pesoBruto,
			DataValidade:      utils.StringToTime(estoqueLocalValores.DataValidade),
			DataFabricacao:    utils.StringToTime(estoqueLocalValores.DataFabricacao),
			UltimoValorPago:   utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
			SeriesProducao:    seriesInput,
			OrdemFabricacao:   ordemProducao.ODF,
		})
	}

	return transferencias, nil
}

func (service *InspecaoSaidaSagaService) PreencherTransferenciasFromInput(input *dto.FinalizarInspecaoInput, ordemProducao *models.OrdemProducao, estoqueLocalValores *models.EstoqueLocalValores, localOrigem int, descricaoOrigem string) ([]dto.InspecaoSaidaTransferenciaBackgroundInputDto, error) {
	transferencias := make([]dto.InspecaoSaidaTransferenciaBackgroundInputDto, 0)
	numerosSeriesJaMovidos := make([]string, 0)

	var pesoLiquido *float64
	var pesoBruto *float64

	if estoqueLocalValores.PesoLiquido == nil {
		pesoLiquido = nil
	} else {
		pesoLiquidoFloat := utils.DecimalToFloat64(*estoqueLocalValores.PesoLiquido)
		pesoLiquido = &pesoLiquidoFloat
	}

	if estoqueLocalValores.PesoBruto == nil {
		pesoBruto = nil
	} else {
		pesoBrutoFloat := utils.DecimalToFloat64(*estoqueLocalValores.PesoBruto)
		pesoBruto = &pesoBrutoFloat
	}

	series, err := service.EstoqueLocalRepository.BuscarSeries(estoqueLocalValores.Recno)
	if err != nil {
		return nil, err
	}

	if input.QuantidadeAprovada > 0 {
		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(input.CodigoLocalAprovado)
		if err != nil {
			return nil, err
		}

		seriesInput := service.GetSeriesInput(series, &numerosSeriesJaMovidos, input.QuantidadeAprovada)
		transferencias = append(transferencias, dto.InspecaoSaidaTransferenciaBackgroundInputDto{
			Quantidade:        input.QuantidadeAprovada,
			LocalOrigem:       localOrigem,
			LocalDestino:      input.CodigoLocalAprovado,
			Documento:         service.GetDocumento(descricaoOrigem, descricaoDestino),
			Fator:             1,
			NumeroPedido:      ordemProducao.NumeroPedido,
			TipoTransferencia: enums.Aprovado,
			PesoLiquido:       pesoLiquido,
			PesoBruto:         pesoBruto,
			DataValidade:      utils.StringToTime(estoqueLocalValores.DataValidade),
			DataFabricacao:    utils.StringToTime(estoqueLocalValores.DataFabricacao),
			UltimoValorPago:   utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
			SeriesProducao:    seriesInput,
			OrdemFabricacao:   ordemProducao.ODF,
		})
	}

	if input.QuantidadeReprovada > 0 {
		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(input.CodigoLocalReprovado)
		if err != nil {
			return nil, err
		}

		seriesInput := service.GetSeriesInput(series, &numerosSeriesJaMovidos, input.QuantidadeReprovada)
		transferencias = append(transferencias, dto.InspecaoSaidaTransferenciaBackgroundInputDto{
			Quantidade:        input.QuantidadeReprovada,
			LocalOrigem:       localOrigem,
			LocalDestino:      input.CodigoLocalReprovado,
			Documento:         service.GetDocumento(descricaoOrigem, descricaoDestino),
			Fator:             1,
			NumeroPedido:      ordemProducao.NumeroPedido,
			TipoTransferencia: enums.Reprovado,
			PesoLiquido:       pesoLiquido,
			PesoBruto:         pesoBruto,
			DataValidade:      utils.StringToTime(estoqueLocalValores.DataValidade),
			DataFabricacao:    utils.StringToTime(estoqueLocalValores.DataFabricacao),
			UltimoValorPago:   utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
			SeriesProducao:    seriesInput,
			OrdemFabricacao:   ordemProducao.ODF,
		})
	}

	if input.QuantidadeRetrabalhada > 0 {
		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(input.CodigoLocalRetrabalho)
		if err != nil {
			return nil, err
		}

		seriesInput := service.GetSeriesInput(series, &numerosSeriesJaMovidos, input.QuantidadeRetrabalhada)
		transferencias = append(transferencias, dto.InspecaoSaidaTransferenciaBackgroundInputDto{
			Quantidade:        input.QuantidadeRetrabalhada,
			LocalOrigem:       localOrigem,
			LocalDestino:      input.CodigoLocalRetrabalho,
			Documento:         service.GetDocumento(descricaoOrigem, descricaoDestino),
			Fator:             1,
			NumeroPedido:      ordemProducao.NumeroPedido,
			TipoTransferencia: enums.Retrabalhado,
			PesoLiquido:       pesoLiquido,
			PesoBruto:         pesoBruto,
			DataValidade:      utils.StringToTime(estoqueLocalValores.DataValidade),
			DataFabricacao:    utils.StringToTime(estoqueLocalValores.DataFabricacao),
			UltimoValorPago:   utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
			SeriesProducao:    seriesInput,
			OrdemFabricacao:   ordemProducao.ODF,
		})
	}

	return transferencias, nil
}

func (service *InspecaoSaidaSagaService) GetResultado(quantidadeLoteDecimal decimal.Decimal, transferencias []dto.InspecaoSaidaTransferenciaBackgroundInputDto) string {
	quantidadeLote := utils.DecimalToFloat64(quantidadeLoteDecimal)

	var quantidadeAprovada float64
	var quantidadeReprovada float64
	var quantidadeRetrabalhada float64

	for _, transferencia := range transferencias {
		if transferencia.TipoTransferencia == enums.Aprovado {
			quantidadeAprovada = transferencia.Quantidade
		}

		if transferencia.TipoTransferencia == enums.Reprovado {
			quantidadeReprovada = transferencia.Quantidade
		}

		if transferencia.TipoTransferencia == enums.Retrabalhado {
			quantidadeRetrabalhada = transferencia.Quantidade
		}
	}

	var resultado string

	if quantidadeReprovada == quantidadeLote || quantidadeRetrabalhada == quantidadeLote {
		resultado = "Não Conforme"
	}

	if quantidadeAprovada > 0 && quantidadeAprovada < quantidadeLote {
		resultado = "Parcialmente Aprov."
	}

	if quantidadeAprovada == quantidadeLote {
		resultado = "Aprovado"
	}

	return resultado
}

func (service *InspecaoSaidaSagaService) GetOdfEstoqueLocal(odf int, localOrigem int) (*models.OrdemProducao, *models.EstoqueLocalValores, error) {
	ordemProducao := service.OrdemProducaoRepository.BuscarOrdem(odf)
	estoqueLocalValores, err := service.EstoqueLocalRepository.BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem)
	if err != nil {
		return nil, nil, err
	}

	return ordemProducao, estoqueLocalValores, err
}

func (service *InspecaoSaidaSagaService) GetDocumento(descricaoOrigem string, descricaoDestino string) string {
	return "Transferencia do local " + descricaoOrigem + " para local " + descricaoDestino + " via inspecao web"
}
