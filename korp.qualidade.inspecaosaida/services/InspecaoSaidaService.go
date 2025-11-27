package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/mappers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/shopspring/decimal"
	"strconv"
	"strings"
	"time"
)

type InspecaoSaidaService struct {
	interfaces.IInspecaoSaidaService
	Uow                         unit_of_work.UnitOfWork
	InspecaoSaidaRepository     interfaces.IInspecaoSaidaRepository
	InspecaoSaidaItemRepository interfaces.IInspecaoSaidaItemRepository
	OrdemProducaoRepository     interfaces.IOrdemProducaoRepository
	PlanosInspecaoRepository    interfaces.IPlanosInspecaoRepository
	BaseParams                  *models.BaseParams
}

func NewInspecaoSaidaService(
	uow unit_of_work.UnitOfWork,
	inspecaoSaidaRepository interfaces.IInspecaoSaidaRepository,
	inspecaoSaidaItemRepository interfaces.IInspecaoSaidaItemRepository,
	ordemProducaoRepository interfaces.IOrdemProducaoRepository,
	planosInspecaoRepository interfaces.IPlanosInspecaoRepository,
	baseParams *models.BaseParams) interfaces.IInspecaoSaidaService {
	return &InspecaoSaidaService{
		Uow:                         uow,
		InspecaoSaidaRepository:     inspecaoSaidaRepository,
		InspecaoSaidaItemRepository: inspecaoSaidaItemRepository,
		OrdemProducaoRepository:     ordemProducaoRepository,
		BaseParams:                  baseParams,
		PlanosInspecaoRepository:    planosInspecaoRepository,
	}
}

func (service *InspecaoSaidaService) BuscarInspecoesSaida(odf int, filter *models.BaseFilter) (*dto.GetInspecaoSaidaDTO, *dto.ValidacaoDTO) {
	inspecoes, err := service.InspecaoSaidaRepository.BuscarInspecoesSaida(odf, filter)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Code:    3,
			Message: err.Error(),
		}
	}

	qtdInspecoes, err := service.InspecaoSaidaRepository.BuscarQuantidadeInspecoesSaida(odf)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Code:    4,
			Message: err.Error(),
		}
	}

	return &dto.GetInspecaoSaidaDTO{
		Items:      mappers.MapInspecaoSaidaEntitiesToDTOs(inspecoes),
		TotalCount: qtdInspecoes,
	}, nil
}

func (service *InspecaoSaidaService) BuscarPlanosNovaInspecao(recnoProcesso int, plano string, filter *models.BaseFilter) (
	*dto.GetPlanosInspecaoDTO, *dto.ValidacaoDTO) {
	planos, err := service.PlanosInspecaoRepository.BuscarPlanosNovaInspecao(recnoProcesso, plano, filter)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Code:    5,
			Message: err.Error(),
		}
	}

	qtdPlanos, err := service.PlanosInspecaoRepository.BuscarQuantidadePlanosNovaInspecao(recnoProcesso, plano)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Code:    6,
			Message: err.Error(),
		}
	}

	return &dto.GetPlanosInspecaoDTO{
		Items:      mappers.MapPlanosInspecaoModelsToDTOs(planos),
		TotalCount: qtdPlanos,
	}, nil
}

func (service *InspecaoSaidaService) CriarInspecao(input *dto.NovaInspecaoInput) (int, *dto.ValidacaoDTO) {
	ordemProducao := service.OrdemProducaoRepository.BuscarOrdem(input.Odf)
	if ordemProducao == nil {
		return 0, &dto.ValidacaoDTO{
			Code:    7,
			Message: "Ordem de produção não encontrada.",
		}
	}

	if input.Quantidade > utils.DecimalToFloat64(ordemProducao.QuantidadeProduzida) {
		return 0, &dto.ValidacaoDTO{
			Code:    28,
			Message: "Atenção a quantidade da inspeção não pode ser maior que a quantidade restante para inspecionar!",
		}
	}

	inspecaoModel := &models.InspecaoSaida{
		CodigoInspecao: service.InspecaoSaidaRepository.BuscarNovoCodigoInspecao(),
		Cliente:        ordemProducao.Cliente,
		Pedido:         ordemProducao.NumeroPedido,
		Odf:            ordemProducao.ODF,
		DataInspecao:   time.Now().Format("20060102"),
		Inspetor:       service.BaseParams.UserLogin,
		QtdInspecao:    decimal.NewFromFloat(input.Quantidade),
		IdEmpresa:      service.BaseParams.CompanyRecno,
		QtdLote:        ordemProducao.QuantidadeLote,
		Lote:           input.Lote,
		CodigoProduto:  input.CodProduto,
	}

	_ = service.Uow.Begin()
	defer service.Uow.UnitOfWorkGuard()
	err := service.InspecaoSaidaRepository.CriarInspecao(inspecaoModel)
	if err != nil {
		_ = service.Uow.Rollback()
		return 0, &dto.ValidacaoDTO{
			Code:    8,
			Message: err.Error(),
		}
	}

	planosInspecaoNaoAlterados, err := service.PlanosInspecaoRepository.BuscarTodosPlanosOdfProduto(ordemProducao.RecnoProcesso, input.Plano)
	if err != nil {
		_ = service.Uow.Rollback()
		return 0, &dto.ValidacaoDTO{
			Code:    9,
			Message: err.Error(),
		}
	}

	var itensInspecao []*models.InspecaoSaidaItem
	planosAlterados := make(map[string]*dto.PlanoInspecaoDTO)
	for _, plano := range input.PlanosInspecao {
		planosAlterados[plano.Id] = plano
	}

	for index, plano := range planosInspecaoNaoAlterados {
		planoAlterado := planosAlterados[plano.Id.String()]
		var item *models.InspecaoSaidaItem

		if planoAlterado != nil && planoAlterado.Id != "" {
			item = &models.InspecaoSaidaItem{
				Plano:          input.Plano,
				Odf:            input.Odf,
				Descricao:      planoAlterado.Descricao,
				Metodo:         planoAlterado.Metodo,
				Sequencia:      strconv.Itoa(index + 1),
				Resultado:      planoAlterado.Resultado,
				MaiorValor:     decimal.NewFromFloat(planoAlterado.MaiorValor),
				MenorValor:     decimal.NewFromFloat(planoAlterado.MenorValor),
				CodigoInspecao: inspecaoModel.CodigoInspecao,
				IdEmpresa:      service.BaseParams.CompanyRecno,
				Observacao:     planoAlterado.Observacao,
			}
		} else {
			item = &models.InspecaoSaidaItem{
				Plano:          input.Plano,
				Odf:            input.Odf,
				Descricao:      plano.Descricao,
				Metodo:         plano.Metodo,
				Sequencia:      strconv.Itoa(index + 1),
				Resultado:      plano.Resultado,
				MaiorValor:     decimal.NewFromFloat(plano.MaiorValor),
				MenorValor:     decimal.NewFromFloat(plano.MenorValor),
				CodigoInspecao: inspecaoModel.CodigoInspecao,
				IdEmpresa:      service.BaseParams.CompanyRecno,
			}
		}

		itensInspecao = append(itensInspecao, item)
	}

	err = service.InspecaoSaidaItemRepository.RemoverInspecaoSaidaItensPeloCodigo(inspecaoModel.CodigoInspecao)
	if err != nil {
		return 0, &dto.ValidacaoDTO{
			Code:    24,
			Message: err.Error(),
		}
	}

	err = service.InspecaoSaidaItemRepository.CriarItensInspecao(itensInspecao)
	if err != nil {
		_ = service.Uow.Rollback()
		return 0, &dto.ValidacaoDTO{
			Code:    10,
			Message: err.Error(),
		}
	}

	_ = service.Uow.Complete()
	return inspecaoModel.CodigoInspecao, nil
}

func (service *InspecaoSaidaService) BuscarInspecaoSaidaPeloCodigo(codigoInspecao int) (*dto.InspecaoSaidaDTO, *dto.ValidacaoDTO) {
	inspecao, err := service.InspecaoSaidaRepository.BuscarInspecaoSaidaDetalhesPeloCodigo(codigoInspecao)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Code:    11,
			Message: err.Error(),
		}
	}

	if inspecao.Resultado != "" {
		return nil, &dto.ValidacaoDTO{
			Code:    26,
			Message: "A inspeção informada já foi finalizada, portanto não é possível alterá-la.",
		}
	}

	return mappers.MapInspecaoSaidaDetalhesToDTO(inspecao), nil
}

func (service *InspecaoSaidaService) BuscarInspecaoSaidaItensPeloCodigo(codigoInspecao int, filter *models.BaseFilter) (*dto.GetInspecaoSaidaItensDTO, *dto.ValidacaoDTO) {
	itens, err := service.InspecaoSaidaItemRepository.BuscarInspecaoSaidaItensPeloCodigo(codigoInspecao, filter)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Code:    12,
			Message: err.Error(),
		}
	}

	qtd, err := service.InspecaoSaidaItemRepository.BuscarQuantidadeInspecaoSaidaItensPeloCodigo(codigoInspecao)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Code:    13,
			Message: err.Error(),
		}
	}

	return &dto.GetInspecaoSaidaItensDTO{
		Items:      mappers.MapInspecaoSaidaItemModelsToDTOs(itens),
		TotalCount: qtd,
	}, nil
}

func (service *InspecaoSaidaService) RemoverInspecaoSaidaPeloCodigo(codigoInspecao int) *dto.ValidacaoDTO {
	inspecao, err := service.InspecaoSaidaRepository.BuscarInspecaoSaidaPeloCodigo(codigoInspecao)
	if err != nil {
		return &dto.ValidacaoDTO{
			Code:    14,
			Message: err.Error(),
		}
	}

	if inspecao.Resultado != "" {
		return &dto.ValidacaoDTO{
			Code:    15,
			Message: "A inspeção informada já foi finalizada, portanto não é possível removê-la.",
		}
	}

	err = service.InspecaoSaidaRepository.RemoverInspecaoSaida(inspecao.Recno)
	if err != nil {
		return &dto.ValidacaoDTO{
			Code:    16,
			Message: err.Error(),
		}
	}

	err = service.InspecaoSaidaItemRepository.RemoverInspecaoSaidaItensPeloCodigo(codigoInspecao)
	if err != nil {
		return &dto.ValidacaoDTO{
			Code:    22,
			Message: err.Error(),
		}
	}

	return nil
}

func (service *InspecaoSaidaService) AtualizarInspecao(input *dto.AtualizarInspecaoInput) *dto.ValidacaoDTO {
	inspecao, err := service.InspecaoSaidaRepository.BuscarInspecaoSaidaPeloCodigo(input.CodInspecao)
	if err != nil {
		return &dto.ValidacaoDTO{
			Code:    17,
			Message: err.Error(),
		}
	}

	if inspecao.Resultado != "" {
		return &dto.ValidacaoDTO{
			Code:    18,
			Message: "A inspeção informada já foi finalizada, portanto não é possível alterá-la.",
		}
	}

	_ = service.Uow.Begin()
	defer service.Uow.UnitOfWorkGuard()

	err = service.InspecaoSaidaRepository.AtualizarQuantidadeInspecaoPeloCodigo(input.CodInspecao, input.QuantidadeInspecao)
	if err != nil {
		_ = service.Uow.Rollback()
		return &dto.ValidacaoDTO{
			Code:    19,
			Message: err.Error(),
		}
	}

	itensEntities, err := service.InspecaoSaidaItemRepository.BuscarInspecaoSaidaItensEntitiesPeloCodigo(input.CodInspecao)
	if err != nil {
		_ = service.Uow.Rollback()
		return &dto.ValidacaoDTO{
			Code:    20,
			Message: err.Error(),
		}
	}

	for _, itemEntity := range itensEntities {
		for _, itemInput := range input.Itens {
			if itemInput.Id == itemEntity.Id.String() {
				itemEntity.MaiorValor = decimal.NewFromFloat(itemInput.MaiorValor)
				itemEntity.MenorValor = decimal.NewFromFloat(itemInput.MenorValor)
				itemEntity.Resultado = itemInput.Resultado
				itemEntity.Observacao = itemInput.Observacao
			}
		}
	}

	err = service.InspecaoSaidaItemRepository.AtualizarInspecaoSaidaItens(itensEntities)
	if err != nil {
		_ = service.Uow.Rollback()
		return &dto.ValidacaoDTO{
			Code:    21,
			Message: err.Error(),
		}
	}

	_ = service.Uow.Complete()
	return nil
}

func (service *InspecaoSaidaService) BuscarResultadoInspecao(codigoInspecao int) (string, *dto.ValidacaoDTO) {
	itens, err := service.InspecaoSaidaItemRepository.BuscarInspecaoSaidaItensEntitiesPeloCodigo(codigoInspecao)
	if err != nil {
		return "", &dto.ValidacaoDTO{
			Code:    25,
			Message: err.Error(),
		}
	}

	temItemAprovado := false
	temItemReprovado := false
	for _, item := range itens {
		if strings.ToLower(item.Resultado) == "não conforme" {
			temItemReprovado = true
		} else if strings.ToLower(item.Resultado) == "aprovado" {
			temItemAprovado = true
		}
	}

	if !temItemAprovado {
		return "Não Conforme", nil
	} else if temItemAprovado && temItemReprovado {
		return "Parcialmente Aprov.", nil
	} else {
		return "Aprovado", nil
	}
}
