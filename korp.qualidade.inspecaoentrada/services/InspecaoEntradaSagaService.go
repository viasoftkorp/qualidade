package services

import (
	"strconv"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/consts"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/enums"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type InspecaoEntradaSagaService struct {
	interfaces.IInspecaoEntradaSagaService
	InspecaoEntradaRepository                interfaces.IInspecaoEntradaRepository
	InspecaoEntradaExecutadoWebRepository    interfaces.IInspecaoEntradaExecutadoWebRepository
	InspecaoEntradaSagaService               interfaces.IExternalInspecaoEntradaSagaService
	NotaFiscalRepository                     interfaces.INotaFiscalRepository
	LocaisRepository                         interfaces.ILocaisRepository
	ParametroRepository                      interfaces.IParametrosRepository
	EstoqueLocalRepository                   interfaces.IEstoquePedidoVendaRepository
	ProdutoRepository                        interfaces.IProdutoRepository
	Uow                                      unit_of_work.UnitOfWork
	InspecaoEntradaPedidoVendaLoteRepository interfaces.IInspecaoEntradaPedidoVendaLoteRepository
}

func NewInspecaoEntradaSagaService(
	inspecaoEntradaRepository interfaces.IInspecaoEntradaRepository,
	inspecaoEntradaExecutadoWebRepository interfaces.IInspecaoEntradaExecutadoWebRepository,
	inspecaoEntradaSagaService interfaces.IExternalInspecaoEntradaSagaService,
	notaFiscalRepository interfaces.INotaFiscalRepository,
	locaisRepository interfaces.ILocaisRepository,
	parametrosRepository interfaces.IParametrosRepository,
	estoqueLocalRepository interfaces.IEstoquePedidoVendaRepository,
	produtoRepository interfaces.IProdutoRepository,
	uow unit_of_work.UnitOfWork,
	inspecaoEntradaPedidoVendaLoteRepository interfaces.IInspecaoEntradaPedidoVendaLoteRepository) interfaces.IInspecaoEntradaSagaService {
	return &InspecaoEntradaSagaService{
		InspecaoEntradaRepository:                inspecaoEntradaRepository,
		InspecaoEntradaExecutadoWebRepository:    inspecaoEntradaExecutadoWebRepository,
		InspecaoEntradaSagaService:               inspecaoEntradaSagaService,
		NotaFiscalRepository:                     notaFiscalRepository,
		LocaisRepository:                         locaisRepository,
		ParametroRepository:                      parametrosRepository,
		EstoqueLocalRepository:                   estoqueLocalRepository,
		ProdutoRepository:                        produtoRepository,
		Uow:                                      uow,
		InspecaoEntradaPedidoVendaLoteRepository: inspecaoEntradaPedidoVendaLoteRepository,
	}
}

func (service *InspecaoEntradaSagaService) RemoverSaga(id string) error {
	err := service.InspecaoEntradaSagaService.RemoverSaga(id)
	if err != nil {
		return err
	}

	err = service.InspecaoEntradaExecutadoWebRepository.RemoverSaga(id)
	if err != nil {
		return err
	}

	return nil
}

func (service *InspecaoEntradaSagaService) ReprocessarSaga(id string) error {
	err := service.InspecaoEntradaSagaService.ReprocessarSaga(id)
	if err != nil {
		return err
	}
	return nil
}

func (service *InspecaoEntradaSagaService) BuscarSagas(baseFilters *models.BaseFilter, filters *dto.ProcessamentoInspecaoEntradaFilters, estorno bool) (*dto.GetAllProcessamentoInspecaoEntradaOutput, error) {
	var output dto.GetAllProcessamentoInspecaoEntradaOutput

	result, err := service.InspecaoEntradaSagaService.BuscarSagas(baseFilters, filters, estorno)
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

		itemProcessamento := dto.ProcessamentoInspecaoEntradaOutput{
			IdSaga:              item.Id,
			Status:              item.Status,
			Erro:                item.Erro,
			NumeroRetentativas:  item.NumeroRetentativas,
			NumeroExecucoes:     item.NumeroExecucoes,
			QuantidadeTotal:     item.QuantidadeTotal,
			Resultado:           item.Resultado,
			CodigoProduto:       item.CodigoProduto,
			DescricaoProduto:    item.CodigoProduto + " - " + descricaoProduto,
			NotaFiscal:          item.NotaFiscal,
			IdUsuarioExecucao:   item.IdUsuarioExecucao,
			NomeUsuarioExecucao: item.NomeUsuarioExecucao,
			DataExecucao:        item.DataExecucao,
			Lote:                item.Lote,
			Estorno:             item.Estorno,
			Transferencias:      make([]dto.ProcessamentoInspecaoEntradaTransferenciaOutput, 0),
		}

		for _, transferencia := range item.Transferencias {
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

			itemProcessamento.Transferencias = append(itemProcessamento.Transferencias, dto.ProcessamentoInspecaoEntradaTransferenciaOutput{
				NumeroPedido:          transferencia.NumeroPedido,
				Quantidade:            transferencia.Quantidade,
				LocalOrigem:           transferencia.LocalOrigem,
				DescricaoLocalOrigem:  strconv.Itoa(transferencia.LocalOrigem) + " - " + descricaoOrigem,
				LocalDestino:          transferencia.LocalDestino,
				DescricaoLocalDestino: strconv.Itoa(transferencia.LocalDestino) + " - " + descricaoDestino,
				TipoTransferencia:     transferencia.TipoTransferencia,
				Lote:                  transferencia.Lote,
				LoteOrigem:            transferencia.LoteOrigem,
			})
		}

		output.Items = append(output.Items, itemProcessamento)
	}

	return &output, nil
}

func (service *InspecaoEntradaSagaService) PublicarSagaInspecaoEntradaEstorno(inspecaoEntradaRecno int) (bool, *dto.ValidacaoDTO, error) {
	inspecaoEntradaExecutadoWeb, err := service.InspecaoEntradaExecutadoWebRepository.BuscarInspecaoEntradaExecutadoWeb(inspecaoEntradaRecno)
	if err != nil {
		return false, nil, err
	}

	saga, err := service.InspecaoEntradaSagaService.BuscarSaga(inspecaoEntradaExecutadoWeb.IdInspecaoEntradaSaga)
	if err != nil {
		return false, nil, err
	}

	movimentarEstoqueInput := dto.MovimentarEstoqueInspecaoBackgroundInputDto{
		IdSaga:        utils.NewGuidAsString(),
		Lote:          saga.Lote,
		Estorno:       true,
		RecnoInspecao: saga.RecnoInspecao,
		CodigoProduto: saga.CodigoProduto,
		NotaFiscal:    saga.NotaFiscal,
		IdRnc:         saga.IdRnc,
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

func (service *InspecaoEntradaSagaService) PreencherTransferenciasEstorno(saga *dto.SagaInspecaoEntradaOutput) ([]dto.InspecaoEntradaTransferenciaBackgroundInputDto, error) {
	transferencias := make([]dto.InspecaoEntradaTransferenciaBackgroundInputDto, 0)

	var pesoLiquido *float64
	var pesoBruto *float64

	for _, transferencia := range saga.Transferencias {
		// Locais e lotes invertidos pois e acao de estorno
		localOrigem := transferencia.LocalDestino
		localDestino := transferencia.LocalOrigem

		loteOrigem := transferencia.Lote
		lote := transferencia.LoteOrigem
		if lote == "" || loteOrigem == "" {
			loteOrigem = saga.Lote
			lote = saga.Lote
		}

		descricaoOrigem, err := service.LocaisRepository.BuscarLocalDescricao(localOrigem)
		if err != nil {
			return nil, err
		}

		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(localDestino)
		if err != nil {
			return nil, err
		}

		estoqueLocalValores, err := service.EstoqueLocalRepository.BuscarEstoqueLocalValoresPorProduto(saga.CodigoProduto, transferencia.Lote, localOrigem)
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

		series := make([]dto.InspecaoEntradaSerieProducaoBackgroundInputDto, 0)
		for _, serie := range transferencia.SeriesProducao {
			series = append(series, dto.InspecaoEntradaSerieProducaoBackgroundInputDto{
				Serie:      serie.Serie,
				RecnoSerie: serie.RecnoSerie,
			})
		}
		transferencias = append(transferencias, dto.InspecaoEntradaTransferenciaBackgroundInputDto{
			Quantidade:         transferencia.Quantidade,
			LocalOrigem:        localOrigem,
			LocalDestino:       localDestino,
			Documento:          service.GetDocumento(descricaoOrigem, descricaoDestino),
			Fator:              1,
			NumeroPedido:       transferencia.NumeroPedido,
			TipoTransferencia:  transferencia.TipoTransferencia,
			PesoLiquido:        pesoLiquido,
			PesoBruto:          pesoBruto,
			DataValidade:       utils.StringToTime(estoqueLocalValores.DataValidade),
			DataFabricacao:     utils.StringToTime(estoqueLocalValores.DataFabricacao),
			UltimoValorPago:    utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
			OrdemFabricacao:    transferencia.OrdemFabricacao,
			Sequencial:         transferencia.Sequencial,
			SeriesProducao:     series,
			Lote:               lote,
			LoteOrigem:         loteOrigem,
			Dimensao1:          transferencia.Dimensao1,
			Dimensao2:          transferencia.Dimensao2,
			Dimensao3:          transferencia.Dimensao3,
			DimensaoDiferenca:  transferencia.DimensaoDiferenca,
			DimensaoQuantidade: transferencia.DimensaoQuantidade,
		})
	}

	return transferencias, nil
}

func (service *InspecaoEntradaSagaService) PublicarSagaInspecaoEntrada(input *dto.FinalizarInspecaoInput) (bool, *dto.ValidacaoDTO, error) {
	utilizarReservaDePedidoNaLocalizacaoDeEstoque, err := service.ParametroRepository.BuscarValorParametro(
		consts.UtilizarReservaDePedidoNaLocalizacaoDeEstoque,
		consts.UtilizarReservaDePedidoNaLocalizacaoDeEstoqueSecao)
	if err != nil {
		return false, nil, err
	}

	if utilizarReservaDePedidoNaLocalizacaoDeEstoque {
		inspecoesEntrada, err := service.InspecaoEntradaRepository.BuscarInspecaoEntradaDetalhesPeloCodigoJoin(input.CodigoInspecao)
		if err != nil {
			return false, nil, err
		}

		notaFiscal, estoqueLocalValores, err := service.GetNotaFiscalEstoqueLocal(inspecoesEntrada[0].RecnoItemNotaFiscal, inspecoesEntrada[0].NotaFiscal, inspecoesEntrada[0].Lote)
		if err != nil {
			return false, nil, err
		}

		caracteristica, err := service.GetCaracteristicaItemNotaFiscal(inspecoesEntrada[0].RecnoItemNotaFiscal, (inspecoesEntrada[0].QuantidadeAprovada.Add(inspecoesEntrada[0].QuantidadeReprovada)))
		if err != nil {
			return false, nil, err
		}

		descricaoOrigem, err := service.LocaisRepository.BuscarLocalDescricao(notaFiscal.CodigoLocal)
		if err != nil {
			return false, nil, err
		}

		movimentarEstoqueInput, err := service.GetMovimentarEstoqueInput(inspecoesEntrada[0].Recno, notaFiscal)
		if err != nil {
			return false, nil, err
		}

		var idsInspecaoEntradaPedidoVendaWeb = make([]uuid.UUID, 0)
		for _, inspecao := range inspecoesEntrada {
			idsInspecaoEntradaPedidoVendaWeb = append(idsInspecaoEntradaPedidoVendaWeb, inspecao.IdInspecaoEntradaPedidoVendaWeb)
		}

		lotesAQuebrarDeTodasAsInpecoes, err := service.InspecaoEntradaPedidoVendaLoteRepository.GetAllByIdPedidoVenda(idsInspecaoEntradaPedidoVendaWeb)

		if err != nil {
			return false, nil, err
		}

		for _, inspecaoPedido := range inspecoesEntrada {
			var lotesAQuebrar = make([]entities.InspecaoEntradaPedidoVendaLote, 0)
			for _, lote := range lotesAQuebrarDeTodasAsInpecoes {
				if lote.IdInspecaoEntradaPedidoVenda == inspecaoPedido.IdInspecaoEntradaPedidoVendaWeb {
					lotesAQuebrar = append(lotesAQuebrar, *lote)
				}
			}

			transferencias, err := service.PreencherTransferenciasFromInspecaoEntrada(&inspecaoPedido, estoqueLocalValores, notaFiscal.CodigoLocal, descricaoOrigem, lotesAQuebrar, caracteristica)
			if err != nil {
				return false, nil, err
			}

			for _, transferencia := range transferencias {
				movimentarEstoqueInput.Transferencias = append(movimentarEstoqueInput.Transferencias, transferencia)
			}
		}

		movimentarEstoqueInput.Resultado = service.GetResultado(inspecoesEntrada[0].QuantidadeLote, movimentarEstoqueInput.Transferencias)
		movimentarEstoqueInput.IdRnc = input.IdRnc
		err = service.PublicarSaga(movimentarEstoqueInput, inspecoesEntrada[0].Recno, false)
		if err != nil {
			return false, nil, err
		}
	} else {
		inspecaoEntrada, err := service.InspecaoEntradaRepository.BuscarInspecaoEntradaDetalhesPeloCodigo(input.CodigoInspecao)
		if err != nil {
			return false, nil, err
		}

		notaFiscal, estoqueLocalValores, err := service.GetNotaFiscalEstoqueLocal(inspecaoEntrada.RecnoItemNotaFiscal, inspecaoEntrada.NotaFiscal, inspecaoEntrada.Lote)
		if err != nil {
			return false, nil, err
		}

		caracteristica, err := service.GetCaracteristicaItemNotaFiscal(inspecaoEntrada.RecnoItemNotaFiscal, inspecaoEntrada.QuantidadeInspecao)
		if err != nil {
			return false, nil, err
		}

		descricaoOrigem, err := service.LocaisRepository.BuscarLocalDescricao(notaFiscal.CodigoLocal)
		if err != nil {
			return false, nil, err
		}

		movimentarEstoqueInput, err := service.GetMovimentarEstoqueInput(inspecaoEntrada.Recno, notaFiscal)
		if err != nil {
			return false, nil, err
		}

		movimentarEstoqueInput.Transferencias, err = service.PreencherTransferenciasFromInput(input, estoqueLocalValores, notaFiscal, descricaoOrigem, caracteristica)
		if err != nil {
			return false, nil, err
		}

		movimentarEstoqueInput.Resultado = service.GetResultado(inspecaoEntrada.QuantidadeLote, movimentarEstoqueInput.Transferencias)
		movimentarEstoqueInput.IdRnc = input.IdRnc
		err = service.PublicarSaga(movimentarEstoqueInput, inspecaoEntrada.Recno, false)
		if err != nil {
			return false, nil, err
		}
	}

	return true, nil, nil
}

func (service *InspecaoEntradaSagaService) PublicarSaga(movimentarEstoqueInput dto.MovimentarEstoqueInspecaoBackgroundInputDto, inspecaoEntradaRecno int, estorno bool) error {
	_ = service.Uow.Begin()
	defer service.Uow.UnitOfWorkGuard()

	inspecaoExecutadoWeb, err := service.InspecaoEntradaExecutadoWebRepository.BuscarInspecaoEntradaExecutadoWeb(inspecaoEntradaRecno)
	if err != nil {
		_ = service.Uow.Rollback()
		return err
	} else if inspecaoExecutadoWeb == nil || inspecaoExecutadoWeb.IdInspecaoEntradaSaga == "" || inspecaoExecutadoWeb.Estorno != estorno {
		inspecaoExecutadaWeb := &entities.InspecaoEntradaExecutadoWeb{
			RecnoInspecaoEntrada:  inspecaoEntradaRecno,
			IdInspecaoEntradaSaga: movimentarEstoqueInput.IdSaga,
			Estorno:               estorno,
			IdRnc:                 movimentarEstoqueInput.IdRnc,
			CodigoProduto:         movimentarEstoqueInput.CodigoProduto,
		}
		err = service.InspecaoEntradaExecutadoWebRepository.InserirInspecaoEntradaExecutadoWeb(inspecaoExecutadaWeb)
		if err != nil {
			_ = service.Uow.Rollback()
			return err
		}
	} else {
		movimentarEstoqueInput.IdSaga = inspecaoExecutadoWeb.IdInspecaoEntradaSaga
	}

	err = service.Uow.Complete()
	if err != nil {
		_ = service.Uow.Rollback()
		return err
	}

	_, err = service.InspecaoEntradaSagaService.PublicarSaga(movimentarEstoqueInput)
	if err != nil {
		return err
	}

	return nil
}

func (service *InspecaoEntradaSagaService) GetSeriesInput(series []models.Serie, numerosSeriesJaMovidos *[]string, quantidadeSaida float64) []dto.InspecaoEntradaSerieProducaoBackgroundInputDto {
	seriesInput := make([]dto.InspecaoEntradaSerieProducaoBackgroundInputDto, 0)

	totalAdicionados := 0.0
	for _, serie := range series {
		if totalAdicionados == quantidadeSaida {
			break
		}
		if utils.Contains(*numerosSeriesJaMovidos, serie.Serie) {
			continue
		}

		totalAdicionados += 1

		if totalAdicionados <= quantidadeSaida {
			seriesInput = append(seriesInput, dto.InspecaoEntradaSerieProducaoBackgroundInputDto{
				Serie:      serie.Serie,
				RecnoSerie: serie.RecnoSerie,
			})

			*numerosSeriesJaMovidos = append(*numerosSeriesJaMovidos, serie.Serie)
		}
	}

	return seriesInput
}

func (service *InspecaoEntradaSagaService) GetMovimentarEstoqueInput(inspecaoEntradaRecno int, notaFiscal *models.NotaFiscalModel) (dto.MovimentarEstoqueInspecaoBackgroundInputDto, error) {
	movimentarEstoqueInput := dto.MovimentarEstoqueInspecaoBackgroundInputDto{
		IdSaga:        utils.NewGuidAsString(),
		Lote:          notaFiscal.Lote,
		Estorno:       false,
		RecnoInspecao: inspecaoEntradaRecno,
		CodigoProduto: notaFiscal.CodigoProduto,
		NotaFiscal:    notaFiscal.NotaFiscal,
	}

	return movimentarEstoqueInput, nil
}

func (service *InspecaoEntradaSagaService) PreencherTransferenciasFromInspecaoEntrada(
	inspecaoEntrada *models.InspecaoEntradaJoin,
	estoqueLocalValores *models.EstoqueLocalValores,
	localOrigem int,
	descricaoOrigem string,
	lotesAQuebrar []entities.InspecaoEntradaPedidoVendaLote,
	caracteristica *models.CaracteristicaItemNotaFiscalModel) ([]dto.InspecaoEntradaTransferenciaBackgroundInputDto, error) {

	transferencias := make([]dto.InspecaoEntradaTransferenciaBackgroundInputDto, 0)
	numerosSeriesJaMovidos := make([]string, 0)

	quantidadeAprovada := utils.DecimalToFloat64(inspecaoEntrada.QuantidadeAprovada)
	quantidadeReprovada := utils.DecimalToFloat64(inspecaoEntrada.QuantidadeReprovada)
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
		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(inspecaoEntrada.CodigoLocalAprovado)
		if err != nil {
			return nil, err
		}

		seriesInput := service.GetSeriesInput(series, &numerosSeriesJaMovidos, quantidadeAprovada)

		for _, lote := range lotesAQuebrar {
			transferencias = append(transferencias, dto.InspecaoEntradaTransferenciaBackgroundInputDto{
				Quantidade:         lote.Quantidade,
				LocalOrigem:        localOrigem,
				LocalDestino:       inspecaoEntrada.CodigoLocalAprovado,
				Documento:          service.GetDocumento(descricaoOrigem, descricaoDestino),
				Fator:              1,
				NumeroPedido:       inspecaoEntrada.NumeroPedido,
				OrdemFabricacao:    inspecaoEntrada.Odf,
				TipoTransferencia:  enums.Aprovado,
				PesoLiquido:        pesoLiquido,
				PesoBruto:          pesoBruto,
				DataValidade:       utils.StringToTime(estoqueLocalValores.DataValidade),
				DataFabricacao:     utils.StringToTime(estoqueLocalValores.DataFabricacao),
				UltimoValorPago:    utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
				SeriesProducao:     seriesInput,
				Lote:               lote.NumeroLote,
				LoteOrigem:         inspecaoEntrada.Lote,
				Dimensao1:          utils.DecimalToFloat64(caracteristica.Dimensao1),
				Dimensao2:          utils.DecimalToFloat64(caracteristica.Dimensao2),
				Dimensao3:          utils.DecimalToFloat64(caracteristica.Dimensao3),
				DimensaoDiferenca:  utils.DecimalToFloat64(caracteristica.Diferenca),
				DimensaoQuantidade: caracteristica.Quantidade,
			})
		}
		somaQuantidadesLotes := 0.0

		for _, item := range lotesAQuebrar {
			somaQuantidadesLotes += item.Quantidade
		}

		var quantidadeManterLote = quantidadeAprovada - somaQuantidadesLotes

		if quantidadeManterLote > 0.0 {
			transferencias = append(transferencias, dto.InspecaoEntradaTransferenciaBackgroundInputDto{
				Quantidade:         quantidadeManterLote,
				LocalOrigem:        localOrigem,
				LocalDestino:       inspecaoEntrada.CodigoLocalAprovado,
				Documento:          service.GetDocumento(descricaoOrigem, descricaoDestino),
				Fator:              1,
				NumeroPedido:       inspecaoEntrada.NumeroPedido,
				OrdemFabricacao:    inspecaoEntrada.Odf,
				TipoTransferencia:  enums.Aprovado,
				PesoLiquido:        pesoLiquido,
				PesoBruto:          pesoBruto,
				DataValidade:       utils.StringToTime(estoqueLocalValores.DataValidade),
				DataFabricacao:     utils.StringToTime(estoqueLocalValores.DataFabricacao),
				UltimoValorPago:    utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
				SeriesProducao:     seriesInput,
				Lote:               inspecaoEntrada.Lote,
				LoteOrigem:         inspecaoEntrada.Lote,
				Dimensao1:          utils.DecimalToFloat64(caracteristica.Dimensao1),
				Dimensao2:          utils.DecimalToFloat64(caracteristica.Dimensao2),
				Dimensao3:          utils.DecimalToFloat64(caracteristica.Dimensao3),
				DimensaoDiferenca:  utils.DecimalToFloat64(caracteristica.Diferenca),
				DimensaoQuantidade: caracteristica.Quantidade,
			})
		}
	}

	if quantidadeReprovada > 0 {
		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(inspecaoEntrada.CodigoLocalReprovado)
		if err != nil {
			return nil, err
		}

		seriesInput := service.GetSeriesInput(series, &numerosSeriesJaMovidos, quantidadeReprovada)

		transferencias = append(transferencias, dto.InspecaoEntradaTransferenciaBackgroundInputDto{
			Quantidade:         quantidadeReprovada,
			LocalOrigem:        localOrigem,
			LocalDestino:       inspecaoEntrada.CodigoLocalReprovado,
			Documento:          service.GetDocumento(descricaoOrigem, descricaoDestino),
			Fator:              1,
			NumeroPedido:       inspecaoEntrada.NumeroPedido,
			OrdemFabricacao:    inspecaoEntrada.Odf,
			TipoTransferencia:  enums.Reprovado,
			PesoLiquido:        pesoLiquido,
			PesoBruto:          pesoBruto,
			DataValidade:       utils.StringToTime(estoqueLocalValores.DataValidade),
			DataFabricacao:     utils.StringToTime(estoqueLocalValores.DataFabricacao),
			UltimoValorPago:    utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
			SeriesProducao:     seriesInput,
			Lote:               inspecaoEntrada.Lote,
			LoteOrigem:         inspecaoEntrada.Lote,
			Dimensao1:          utils.DecimalToFloat64(caracteristica.Dimensao1),
			Dimensao2:          utils.DecimalToFloat64(caracteristica.Dimensao2),
			Dimensao3:          utils.DecimalToFloat64(caracteristica.Dimensao3),
			DimensaoDiferenca:  utils.DecimalToFloat64(caracteristica.Diferenca),
			DimensaoQuantidade: caracteristica.Quantidade,
		})
	}

	return transferencias, nil
}

func (service *InspecaoEntradaSagaService) PreencherTransferenciasFromInput(input *dto.FinalizarInspecaoInput, estoqueLocalValores *models.EstoqueLocalValores, notaFiscal *models.NotaFiscalModel, descricaoOrigem string, caracteristica *models.CaracteristicaItemNotaFiscalModel) ([]dto.InspecaoEntradaTransferenciaBackgroundInputDto, error) {
	transferencias := make([]dto.InspecaoEntradaTransferenciaBackgroundInputDto, 0)
	numerosSeriesJaMovidos := make([]string, 0)

	var pesoLiquido *float64
	var pesoBruto *float64
	var dataValidade string
	var dataFabricacao string
	var valorPago decimal.Decimal
	var estoqueLocalRecno int

	if estoqueLocalValores == nil {
		dataValidade = ""
		dataFabricacao = ""
		valorPago = decimal.Zero
		estoqueLocalRecno = 0
	} else {
		dataValidade = estoqueLocalValores.DataValidade
		dataFabricacao = estoqueLocalValores.DataFabricacao
		valorPago = estoqueLocalValores.ValorPago
		estoqueLocalRecno = estoqueLocalValores.Recno
	}

	if estoqueLocalValores == nil || estoqueLocalValores.PesoLiquido == nil {
		pesoLiquido = nil
	} else {
		pesoLiquidoFloat := utils.DecimalToFloat64(*estoqueLocalValores.PesoLiquido)
		pesoLiquido = &pesoLiquidoFloat
	}

	if estoqueLocalValores == nil || estoqueLocalValores.PesoBruto == nil {
		pesoBruto = nil
	} else {
		pesoBrutoFloat := utils.DecimalToFloat64(*estoqueLocalValores.PesoBruto)
		pesoBruto = &pesoBrutoFloat
	}

	series, err := service.EstoqueLocalRepository.BuscarSeries(estoqueLocalRecno)
	if err != nil {
		return nil, err
	}

	if input.QuantidadeAprovada > 0 {
		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(input.CodigoLocalPrincipal)
		if err != nil {
			return nil, err
		}

		seriesInput := service.GetSeriesInput(series, &numerosSeriesJaMovidos, input.QuantidadeAprovada)
		for _, lote := range input.Lotes {
			transferencias = append(transferencias, dto.InspecaoEntradaTransferenciaBackgroundInputDto{
				Quantidade:         lote.Quantidade,
				LocalOrigem:        notaFiscal.CodigoLocal,
				LocalDestino:       input.CodigoLocalPrincipal,
				Documento:          service.GetDocumento(descricaoOrigem, descricaoDestino),
				Fator:              1,
				TipoTransferencia:  enums.Aprovado,
				PesoLiquido:        pesoLiquido,
				PesoBruto:          pesoBruto,
				DataValidade:       utils.StringToTime(dataValidade),
				DataFabricacao:     utils.StringToTime(dataFabricacao),
				UltimoValorPago:    utils.DecimalToFloat64(valorPago),
				SeriesProducao:     seriesInput,
				Lote:               lote.NumeroLote,
				LoteOrigem:         notaFiscal.Lote,
				Dimensao1:          utils.DecimalToFloat64(caracteristica.Dimensao1),
				Dimensao2:          utils.DecimalToFloat64(caracteristica.Dimensao2),
				Dimensao3:          utils.DecimalToFloat64(caracteristica.Dimensao3),
				DimensaoDiferenca:  utils.DecimalToFloat64(caracteristica.Diferenca),
				DimensaoQuantidade: caracteristica.Quantidade,
			})
		}
		somaQuantidadesLotes := 0.0

		for _, item := range input.Lotes {
			somaQuantidadesLotes += item.Quantidade
		}

		var quantidadeManterLote = input.QuantidadeAprovada - somaQuantidadesLotes
		if quantidadeManterLote > 0.0 {
			transferencias = append(transferencias, dto.InspecaoEntradaTransferenciaBackgroundInputDto{
				Quantidade:         quantidadeManterLote,
				LocalOrigem:        notaFiscal.CodigoLocal,
				LocalDestino:       input.CodigoLocalPrincipal,
				Documento:          service.GetDocumento(descricaoOrigem, descricaoDestino),
				Fator:              1,
				TipoTransferencia:  enums.Aprovado,
				PesoLiquido:        pesoLiquido,
				PesoBruto:          pesoBruto,
				DataValidade:       utils.StringToTime(dataValidade),
				DataFabricacao:     utils.StringToTime(dataFabricacao),
				UltimoValorPago:    utils.DecimalToFloat64(valorPago),
				SeriesProducao:     seriesInput,
				Lote:               notaFiscal.Lote,
				LoteOrigem:         notaFiscal.Lote,
				Dimensao1:          utils.DecimalToFloat64(caracteristica.Dimensao1),
				Dimensao2:          utils.DecimalToFloat64(caracteristica.Dimensao2),
				Dimensao3:          utils.DecimalToFloat64(caracteristica.Dimensao3),
				DimensaoDiferenca:  utils.DecimalToFloat64(caracteristica.Diferenca),
				DimensaoQuantidade: caracteristica.Quantidade,
			})
		}
	}

	if input.QuantidadeRejeitada > 0 {
		descricaoDestino, err := service.LocaisRepository.BuscarLocalDescricao(input.CodigoLocalReprovado)
		if err != nil {
			return nil, err
		}

		seriesInput := service.GetSeriesInput(series, &numerosSeriesJaMovidos, input.QuantidadeRejeitada)

		transferencias = append(transferencias, dto.InspecaoEntradaTransferenciaBackgroundInputDto{
			Quantidade:         input.QuantidadeRejeitada,
			LocalOrigem:        notaFiscal.CodigoLocal,
			LocalDestino:       input.CodigoLocalReprovado,
			Documento:          service.GetDocumento(descricaoOrigem, descricaoDestino),
			Fator:              1,
			TipoTransferencia:  enums.Reprovado,
			PesoLiquido:        pesoLiquido,
			PesoBruto:          pesoBruto,
			DataValidade:       utils.StringToTime(estoqueLocalValores.DataValidade),
			DataFabricacao:     utils.StringToTime(estoqueLocalValores.DataFabricacao),
			UltimoValorPago:    utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
			SeriesProducao:     seriesInput,
			Lote:               notaFiscal.Lote,
			LoteOrigem:         notaFiscal.Lote,
			Dimensao1:          utils.DecimalToFloat64(caracteristica.Dimensao1),
			Dimensao2:          utils.DecimalToFloat64(caracteristica.Dimensao2),
			Dimensao3:          utils.DecimalToFloat64(caracteristica.Dimensao3),
			DimensaoDiferenca:  utils.DecimalToFloat64(caracteristica.Diferenca),
			DimensaoQuantidade: caracteristica.Quantidade,
		})
	}

	return transferencias, nil
}

func (service *InspecaoEntradaSagaService) GetResultado(quantidadeLoteDecimal decimal.Decimal, transferencias []dto.InspecaoEntradaTransferenciaBackgroundInputDto) string {
	quantidadeLote := utils.DecimalToFloat64(quantidadeLoteDecimal)

	var quantidadeAprovada float64
	var quantidadeReprovada float64

	for _, transferencia := range transferencias {
		if transferencia.TipoTransferencia == enums.Aprovado {
			quantidadeAprovada = transferencia.Quantidade
		}

		if transferencia.TipoTransferencia == enums.Reprovado {
			quantidadeReprovada = transferencia.Quantidade
		}
	}

	var resultado string

	if quantidadeReprovada == quantidadeLote {
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

func (service *InspecaoEntradaSagaService) GetNotaFiscalEstoqueLocal(recnoNotaFiscal int, codigoNotaFiscal int, lote string) (*models.NotaFiscalModel, *models.EstoqueLocalValores, error) {
	notaFiscal, _ := service.NotaFiscalRepository.BuscarNotaFiscal(recnoNotaFiscal, codigoNotaFiscal, lote)
	estoqueLocalValores, err := service.EstoqueLocalRepository.BuscarEstoqueLocalValoresPorProduto(notaFiscal.CodigoProduto, notaFiscal.Lote, notaFiscal.CodigoLocal)
	if err != nil {
		return nil, nil, err
	}

	return &notaFiscal, estoqueLocalValores, err
}

func (service *InspecaoEntradaSagaService) GetCaracteristicaItemNotaFiscal(recnoNotaFiscal int, quantidade decimal.Decimal) (*models.CaracteristicaItemNotaFiscalModel, error) {
	caracteristica, err := service.NotaFiscalRepository.BuscarCaracteristicaItemNotaFiscal(recnoNotaFiscal, quantidade)
	if err != nil {
		return nil, err
	}

	return &caracteristica, err
}

func (service *InspecaoEntradaSagaService) GetDocumento(descricaoOrigem string, descricaoDestino string) string {
	return "Transferencia do lugar " + descricaoOrigem + " para lugar " + descricaoDestino + " via inspecao web"
}

func (service *InspecaoEntradaSagaService) GetTipoTransferenciaInspecaoString(resultado enums.TipoTransferencia) string {
	if resultado == enums.Aprovado {
		return "Aprovado"
	}
	return "Reprovado"
}
