package services

import (
	"encoding/base64"
	"strconv"
	"strings"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/consts"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/mappers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/shopspring/decimal"
)

type InspecaoEntradaService struct {
	interfaces.IInspecaoEntradaService
	Uow                           unit_of_work.UnitOfWork
	InspecaoEntradaRepository     interfaces.IInspecaoEntradaRepository
	InspecaoEntradaItemRepository interfaces.IInspecaoEntradaItemRepository
	NotaFiscalRepository          interfaces.INotaFiscalRepository
	PlanosInspecaoRepository      interfaces.IPlanosInspecaoRepository
	ParametrosRepository          interfaces.IParametrosRepository
	LocaisRepository              interfaces.ILocaisRepository
	EstoquePedidoVendaRepository  interfaces.IEstoquePedidoVendaRepository
	ExternalMovimentacaoService   interfaces.IExternalMovimentacaoService
	ImpressaoService              interfaces.IImpressaoService
	EmpresaRepository             interfaces.IEmpresaRepository
	BaseParams                    *models.BaseParams
}

func NewInspecaoEntradaService(
	uow unit_of_work.UnitOfWork,
	inspecaoEntradaRepository interfaces.IInspecaoEntradaRepository,
	inspecaoEntradaItemRepository interfaces.IInspecaoEntradaItemRepository,
	notaFiscalRepository interfaces.INotaFiscalRepository,
	planosInspecaoRepository interfaces.IPlanosInspecaoRepository,
	parametrosRepository interfaces.IParametrosRepository,
	locaisRepository interfaces.ILocaisRepository,
	estoquePedidoVendaRepository interfaces.IEstoquePedidoVendaRepository,
	externalMovimentacaoService interfaces.IExternalMovimentacaoService,
	baseParams *models.BaseParams,
	impressaoService interfaces.IImpressaoService,
	empresaRepository interfaces.IEmpresaRepository) interfaces.IInspecaoEntradaService {
	return &InspecaoEntradaService{
		Uow:                           uow,
		InspecaoEntradaRepository:     inspecaoEntradaRepository,
		InspecaoEntradaItemRepository: inspecaoEntradaItemRepository,
		NotaFiscalRepository:          notaFiscalRepository,
		BaseParams:                    baseParams,
		PlanosInspecaoRepository:      planosInspecaoRepository,
		ParametrosRepository:          parametrosRepository,
		LocaisRepository:              locaisRepository,
		EstoquePedidoVendaRepository:  estoquePedidoVendaRepository,
		ExternalMovimentacaoService:   externalMovimentacaoService,
		ImpressaoService:              impressaoService,
		EmpresaRepository:             empresaRepository,
	}
}

func (service *InspecaoEntradaService) BuscarInspecoesEntrada(notaFiscal int, lote string, baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaFilters) (*dto.GetInspecaoEntradaDTO, *dto.ValidacaoDTO, error) {
	inspecoes, err := service.InspecaoEntradaRepository.BuscarInspecoesEntrada(notaFiscal, lote, baseFilters, filters)
	if err != nil {
		return nil, nil, err
	}

	quantidadeInspecoes, err := service.InspecaoEntradaRepository.BuscarQuantidadeInspecoesEntrada(notaFiscal, lote, baseFilters, filters)
	if err != nil {
		return nil, nil, err
	}

	return &dto.GetInspecaoEntradaDTO{
		Items:      mappers.MapInspecaoEntradaEntitiesToDTOs(inspecoes),
		TotalCount: quantidadeInspecoes,
	}, nil, nil
}

func (service *InspecaoEntradaService) BuscarPlanosNovaInspecao(plano string, codigoProduto string, filter *models.BaseFilter) (*dto.GetPlanosInspecaoDTO, *dto.ValidacaoDTO, error) {
	planos, err := service.PlanosInspecaoRepository.BuscarPlanosNovaInspecao(plano, codigoProduto, filter)
	if err != nil {
		return nil, nil, err
	}

	quantidadePlanos, err := service.PlanosInspecaoRepository.BuscarQuantidadePlanosNovaInspecao(plano, codigoProduto)
	if err != nil {
		return nil, nil, err
	}

	return &dto.GetPlanosInspecaoDTO{
		Items:      mappers.MapPlanosInspecaoModelsToDTOs(planos),
		TotalCount: quantidadePlanos,
	}, nil, nil
}

func (service *InspecaoEntradaService) CriarInspecao(input *dto.NovaInspecaoInput) (int, *dto.ValidacaoDTO, error) {
	notaFiscal, err := service.NotaFiscalRepository.BuscarNotaFiscal(input.RecnoItemNotaFiscal, input.NotaFiscal, input.Lote)
	if err != nil {
		return 0, nil, err
	}
	if notaFiscal.NotaFiscal == 0 {
		return 0, &dto.ValidacaoDTO{
			Code:    1,
			Message: "Nota Fiscal não encontrada.",
		}, nil
	}

	inspecaoModel := &models.InspecaoEntrada{
		CodigoInspecao:      service.InspecaoEntradaRepository.BuscarNovoCodigoInspecao(),
		DataInspecao:        time.Now().Format("20060102"),
		NotaFiscal:          notaFiscal.NotaFiscal,
		Fornecedor:          notaFiscal.DescricaoForneced,
		Inspetor:            service.BaseParams.UserLogin,
		QuantidadeInspecao:  decimal.NewFromFloat(input.Quantidade),
		Lote:                notaFiscal.Lote,
		QuantidadeLote:      notaFiscal.Quantidade,
		IdEmpresa:           service.BaseParams.LegacyCompanyId,
		SerieNotaFiscal:     notaFiscal.Serie,
		RecnoItemNotaFiscal: notaFiscal.Recno,
		CodigoProduto:       notaFiscal.CodigoProduto,
	}

	_ = service.Uow.Begin()
	defer service.Uow.UnitOfWorkGuard()

	err = service.InspecaoEntradaRepository.CriarInspecao(inspecaoModel)
	if err != nil {
		_ = service.Uow.Rollback()
		return 0, nil, err
	}

	planosInspecaoNaoAlterados, err := service.PlanosInspecaoRepository.BuscarTodosPlanosNotaFiscalProduto(input.Plano, input.CodigoProduto)
	if err != nil {
		_ = service.Uow.Rollback()
		return 0, nil, err
	}

	var itensInspecao []models.InspecaoEntradaItem
	planosAlterados := make(map[string]*dto.PlanoInspecaoDTO)

	for _, plano := range input.PlanosInspecao {
		planosAlterados[plano.Id] = plano
	}

	for index, plano := range planosInspecaoNaoAlterados {
		planoAlterado := planosAlterados[plano.Id.String()]
		var item models.InspecaoEntradaItem

		if planoAlterado != nil && planoAlterado.Id != "" {
			item = models.InspecaoEntradaItem{
				LegacyIdPlanoInspecao:  plano.LegacyId,
				Plano:                  input.Plano,
				Descricao:              planoAlterado.Descricao,
				Metodo:                 planoAlterado.Metodo,
				Sequencia:              strconv.Itoa(index + 1),
				Resultado:              planoAlterado.Resultado,
				MaiorValorInspecionado: decimal.NewFromFloat(planoAlterado.MaiorValorInspecionado),
				MenorValorInspecionado: decimal.NewFromFloat(planoAlterado.MenorValorInspecionado),
				CodigoInspecao:         inspecaoModel.CodigoInspecao,
				Observacao:             planoAlterado.Observacao,
			}
		} else {
			item = models.InspecaoEntradaItem{
				LegacyIdPlanoInspecao:  plano.LegacyId,
				Plano:                  input.Plano,
				Descricao:              plano.Descricao,
				Metodo:                 plano.Metodo,
				Sequencia:              strconv.Itoa(index + 1),
				Resultado:              plano.Resultado,
				MaiorValorInspecionado: decimal.NewFromFloat(plano.MaiorValorInspecionado),
				MenorValorInspecionado: decimal.NewFromFloat(plano.MenorValorInspecionado),
				CodigoInspecao:         inspecaoModel.CodigoInspecao,
			}
		}

		itensInspecao = append(itensInspecao, item)
	}

	err = service.InspecaoEntradaItemRepository.RemoverInspecaoEntradaItensPeloCodigo(inspecaoModel.CodigoInspecao)
	if err != nil {
		return 0, nil, err
	}

	err = service.InspecaoEntradaItemRepository.CriarItensInspecao(itensInspecao)
	if err != nil {
		_ = service.Uow.Rollback()
		return 0, nil, err
	}

	_ = service.Uow.Complete()
	return inspecaoModel.CodigoInspecao, nil, nil
}

func (service *InspecaoEntradaService) BuscarInspecaoEntradaPeloCodigo(codigoInspecao int) (dto.InspecaoEntradaDTO, *dto.ValidacaoDTO, error) {
	inspecao, err := service.InspecaoEntradaRepository.BuscarInspecaoEntradaPeloCodigo(codigoInspecao)
	if err != nil {
		return *new(dto.InspecaoEntradaDTO), nil, err
	}

	if inspecao.Resultado != "" {
		return *new(dto.InspecaoEntradaDTO), &dto.ValidacaoDTO{
			Code:    26,
			Message: "A inspeção informada já foi finalizada, portanto não é possível alterá-la.",
		}, nil
	}

	return mappers.MapInspecaoEntradaEntityToDTO(inspecao), nil, nil
}

func (service *InspecaoEntradaService) BuscarInspecaoEntradaItensPeloCodigo(codigoProduto string, codigoInspecao int, filter *models.BaseFilter) (*dto.GetInspecaoEntradaItensDTO, *dto.ValidacaoDTO, error) {
	itens, err := service.InspecaoEntradaItemRepository.BuscarInspecaoEntradaItensPeloCodigo(codigoProduto, codigoInspecao, filter)
	if err != nil {
		return nil, nil, err
	}

	quantidade, err := service.InspecaoEntradaItemRepository.BuscarQuantidadeInspecaoEntradaItensPeloCodigo(codigoInspecao)
	if err != nil {
		return nil, nil, err
	}

	return &dto.GetInspecaoEntradaItensDTO{
		Items:      mappers.MapInspecaoEntradaItemModelsToDTOs(itens),
		TotalCount: quantidade,
	}, nil, nil
}

func (service *InspecaoEntradaService) RemoverInspecaoEntradaPeloCodigo(codigoInspecao int) (*dto.ValidacaoDTO, error) {
	inspecao, err := service.InspecaoEntradaRepository.BuscarInspecaoEntradaPeloCodigo(codigoInspecao)
	if err != nil {
		return nil, err
	}

	if inspecao.Resultado != "" {
		return &dto.ValidacaoDTO{
			Code:    15,
			Message: "A inspeção informada já foi finalizada, portanto não é possível removê-la.",
		}, nil
	}

	err = service.InspecaoEntradaRepository.RemoverInspecaoEntrada(inspecao.Recno)
	if err != nil {
		return nil, err
	}

	err = service.InspecaoEntradaItemRepository.RemoverInspecaoEntradaItensPeloCodigo(codigoInspecao)
	if err != nil {
		return nil, err
	}

	utilizaReserva, err := service.ParametrosRepository.BuscarValorParametro("UtilizarReservaDePedidoNaLocalizacaoDeEstoque", "Logistica")
	if err != nil {
		return nil, err
	}
	if utilizaReserva {

		err = service.EstoquePedidoVendaRepository.RemoverInspecaoEntradaPedidoVenda(inspecao.Recno)
		if err != nil {
			return nil, err
		}
	}

	return nil, nil
}

func (service *InspecaoEntradaService) AtualizarInspecao(input *dto.AtualizarInspecaoInput) (*dto.ValidacaoDTO, error) {
	inspecao, err := service.InspecaoEntradaRepository.BuscarInspecaoEntradaPeloCodigo(input.CodigoInspecao)
	if err != nil {
		return nil, err
	}

	if inspecao.Resultado != "" {
		return &dto.ValidacaoDTO{
			Code:    18,
			Message: "A inspeção informada já foi finalizada, portanto não é possível alterá-la.",
		}, nil
	}

	itensEntities, err := service.InspecaoEntradaItemRepository.BuscarInspecaoEntradaItensEntitiesPeloCodigo(input.CodigoInspecao)
	if err != nil {
		_ = service.Uow.Rollback()
		return nil, err
	}

	_ = service.Uow.Begin()
	defer service.Uow.UnitOfWorkGuard()

	err = service.InspecaoEntradaRepository.AtualizarQuantidadeInspecaoPeloCodigo(input.CodigoInspecao, input.QuantidadeInspecao)
	if err != nil {
		_ = service.Uow.Rollback()
		return nil, err
	}

	for _, itemEntity := range itensEntities {
		for _, itemInput := range input.Itens {
			if itemInput.Id == itemEntity.Id.String() {
				itemEntity.MaiorValorInspecionado = decimal.NewFromFloat(itemInput.MaiorValorInspecionado)
				itemEntity.MenorValorInspecionado = decimal.NewFromFloat(itemInput.MenorValorInspecionado)
				itemEntity.Resultado = itemInput.Resultado
				itemEntity.Observacao = itemInput.Observacao
			}
		}
	}

	err = service.InspecaoEntradaItemRepository.AtualizarInspecaoEntradaItens(itensEntities)
	if err != nil {
		_ = service.Uow.Rollback()
		return nil, err
	}

	_ = service.Uow.Complete()
	return nil, nil
}

func (service *InspecaoEntradaService) BuscarResultadoInspecao(codigoInspecao int) (string, *dto.ValidacaoDTO, error) {
	itens, err := service.InspecaoEntradaItemRepository.BuscarInspecaoEntradaItensEntitiesPeloCodigo(codigoInspecao)
	if err != nil {
		return "", nil, err
	}

	for _, item := range itens {
		if strings.ToLower(item.Resultado) == "não conforme" {
			return consts.InspecaoNaoConforme, nil, nil
		}
	}

	return consts.InspecaoAprovada, nil, nil
}

func (service *InspecaoEntradaService) BuscarValorParametro(chaveParametro, secao string) (bool, error) {
	return service.ParametrosRepository.BuscarValorParametro(chaveParametro, secao)
}

func (service *InspecaoEntradaService) ImprimirInspecaoEntrada(codigoInspecao int) ([]byte, *dto.ValidacaoDTO) {
	inspecao, err := service.InspecaoEntradaRepository.BuscarInspecaoEntradaPeloCodigo(codigoInspecao)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Message: err.Error(),
		}
	}

	nota, err := service.NotaFiscalRepository.BuscarNotaFiscal(inspecao.RecnoItemNotaFiscal, inspecao.NotaFiscal, inspecao.Lote)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Message: err.Error(),
		}
	}

	dataFabricacao, err := convertStringToDate(nota.DataFabricacao)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Message: err.Error(),
		}
	}

	dataValidade, err := convertStringToDate(nota.DataValidade)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Message: err.Error(),
		}
	}

	dataInspecao, err := convertStringToDate(inspecao.DataInspecao)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Message: err.Error(),
		}
	}

	itensInspecao, err := service.InspecaoEntradaItemRepository.BuscarInspecaoEntradaItensPeloCodigo(nota.CodigoProduto, codigoInspecao, &models.BaseFilter{
		Filter:         "",
		AdvancedFilter: "",
		Sorting:        "",
		Skip:           0,
		PageSize:       0,
	})
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Message: err.Error(),
		}
	}

	logoEmpresa, err := service.EmpresaRepository.BuscarLogo(nota.IdEmpresa)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Message: err.Error(),
		}
	}

	itensInspecaoRelatorio := make([]dto.ItemInspecaoRelatorio, len(itensInspecao))

	for i, itemInspecao := range itensInspecao {
		itensInspecaoRelatorio[i] = dto.ItemInspecaoRelatorio{
			Id:             itemInspecao.Id,
			CodigoInspecao: itemInspecao.CodigoInspecao,
			Descricao:      itemInspecao.Descricao,
			Metodo:         itemInspecao.Metodo,
			Sequencia:      itemInspecao.Sequencia,
			Resultado:      itemInspecao.Resultado,
			MaiorValor:     itemInspecao.MaiorValorInspecionado,
			MenorValor:     itemInspecao.MenorValorInspecionado,
			MaiorValorBase: itemInspecao.MaiorValorBase,
			MenorValorBase: itemInspecao.MenorValorBase,
			Observacao:     itemInspecao.Observacao,
		}
	}

	timeNow := time.Now().UTC()
	dataEmissao := utils.SetReportingDateTimeZone(&timeNow).Format("2006-01-02 15:04:05")

	exportReportInput := dto.ExportarRelatorioInput{
		Data: &dto.ExportarRelatorioData{
			Inspecao: &dto.InspecaoDataSource{
				Inspecao: []dto.InspecaoRelatorio{
					{
						LogoEmpresa:         base64.StdEncoding.EncodeToString(logoEmpresa),
						DataEmissao:         dataEmissao,
						CodigoProduto:       nota.CodigoProduto,
						DescricaoProduto:    nota.DescricaoProduto,
						DataFabricacao:      dataFabricacao,
						DataValidade:        dataValidade,
						IdEmpresa:           nota.IdEmpresa,
						Usuario:             service.BaseParams.UserLogin,
						Recno:               inspecao.Recno,
						CodigoInspecao:      inspecao.CodigoInspecao,
						NotaFiscal:          nota.NotaFiscal,
						SerieNotaFiscal:     nota.Serie,
						Inspecionado:        inspecao.Inspecionado,
						DataInspecao:        dataInspecao,
						Inspetor:            inspecao.Inspetor,
						Resultado:           inspecao.Resultado,
						Lote:                inspecao.Lote,
						QuantidadeInspecao:  inspecao.QuantidadeInspecao,
						QuantidadeLote:      inspecao.QuantidadeLote,
						QuantidadeAceita:    inspecao.QuantidadeAceita,
						QuantidadeAprovada:  inspecao.QuantidadeAprovada,
						QuantidadeReprovada: inspecao.QuantidadeReprovada,
						Id:                  inspecao.Id,
					},
				},
			},
			ItemInspecao: &dto.ItemInspecaoDataSource{
				ItemInspecao: itensInspecaoRelatorio,
			},
		},
		ReportingOutputType: "pdf",
	}

	reportOutput, err := service.ImpressaoService.ExportReportStimulsoft(exportReportInput, consts.InspecaoReportId)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Message: err.Error(),
		}
	}

	return reportOutput, nil
}

func convertStringToDate(dateStr string) (*time.Time, error) {
	if dateStr == "" {
		return nil, nil
	}
	layout := "20060102" // Formato: yyyyMMdd

	date, err := time.Parse(layout, dateStr)
	if err != nil {
		return nil, err
	}

	return &date, nil
}
