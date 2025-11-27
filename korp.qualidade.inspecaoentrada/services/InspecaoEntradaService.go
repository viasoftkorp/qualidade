package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/consts"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/mappers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/shopspring/decimal"
	"strconv"
	"strings"
	"time"
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
	baseParams *models.BaseParams) interfaces.IInspecaoEntradaService {
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
	}
}

func (service *InspecaoEntradaService) BuscarInspecoesEntrada(notaFiscal int, lote string, filter *models.BaseFilter) (*dto.GetInspecaoEntradaDTO, *dto.ValidacaoDTO, error) {
	inspecoes, err := service.InspecaoEntradaRepository.BuscarInspecoesEntrada(notaFiscal, lote, filter)
	if err != nil {
		return nil, nil, err
	}

	quantidadeInspecoes, err := service.InspecaoEntradaRepository.BuscarQuantidadeInspecoesEntrada(notaFiscal, lote)
	if err != nil {
		return nil, nil, err
	}

	return &dto.GetInspecaoEntradaDTO{
		Items:      mappers.MapInspecaoEntradaEntitiesToDTOs(inspecoes),
		TotalCount: quantidadeInspecoes,
	}, nil, nil
}

func (service *InspecaoEntradaService) BuscarPlanosNovaInspecao(plano int, codigoProduto string, filter *models.BaseFilter) (*dto.GetPlanosInspecaoDTO, *dto.ValidacaoDTO, error) {
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
	notaFiscal, err := service.NotaFiscalRepository.BuscarNotaFiscal(input.NotaFiscal, input.Lote)
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
		CodigoInspecao:     service.InspecaoEntradaRepository.BuscarNovoCodigoInspecao(),
		DataInspecao:       time.Now().Format("20060102"),
		NotaFiscal:         notaFiscal.NotaFiscal,
		Fornecedor:         notaFiscal.DescricaoForneced,
		Inspetor:           service.BaseParams.UserLogin,
		QuantidadeInspecao: decimal.NewFromFloat(input.Quantidade),
		Lote:               input.Lote,
		QuantidadeLote:     notaFiscal.Quantidade,
		IdEmpresa:          service.BaseParams.CompanyRecno,
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

	_ = service.Uow.Begin()
	defer service.Uow.UnitOfWorkGuard()

	err = service.InspecaoEntradaRepository.AtualizarQuantidadeInspecaoPeloCodigo(input.CodigoInspecao, input.QuantidadeInspecao)
	if err != nil {
		_ = service.Uow.Rollback()
		return nil, err
	}

	itensEntities, err := service.InspecaoEntradaItemRepository.BuscarInspecaoEntradaItensEntitiesPeloCodigo(input.CodigoInspecao)
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
