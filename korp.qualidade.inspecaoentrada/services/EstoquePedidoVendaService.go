package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/google/uuid"
)

type EstoqueLocalPedidoVendaService struct {
	interfaces.IEstoquePedidoVendaService
	Uow                                      unit_of_work.UnitOfWork
	EstoquePedidoVendaRepository             interfaces.IEstoquePedidoVendaRepository
	LocaisRepository                         interfaces.ILocaisRepository
	InspecaoEntradaRepository                interfaces.IInspecaoEntradaRepository
	InspecaoEntradaPedidoVendaLoteRepository interfaces.IInspecaoEntradaPedidoVendaLoteRepository
}

func NewEstoqueLocalPedidoVendaService(estoquePedidoVendaRepository interfaces.IEstoquePedidoVendaRepository,
	locaisRepository interfaces.ILocaisRepository,
	inspecaoEntradaRepository interfaces.IInspecaoEntradaRepository,
	inspecaoEntradaPedidoVendaLoteRepository interfaces.IInspecaoEntradaPedidoVendaLoteRepository,
	uow unit_of_work.UnitOfWork) interfaces.IEstoquePedidoVendaService {
	return &EstoqueLocalPedidoVendaService{
		EstoquePedidoVendaRepository:             estoquePedidoVendaRepository,
		LocaisRepository:                         locaisRepository,
		InspecaoEntradaRepository:                inspecaoEntradaRepository,
		InspecaoEntradaPedidoVendaLoteRepository: inspecaoEntradaPedidoVendaLoteRepository,
		Uow:                                      uow,
	}
}

func (service *EstoqueLocalPedidoVendaService) BuscarQuantidadeTotalAlocadaPedidoVenda(recnoInspecaoEntrada int) (*dto.EstoqueLocalPedidoVendoTotalizacaoDTO, error) {
	return service.EstoquePedidoVendaRepository.BuscarQuantidadeTotalAlocadaPedidoVenda(recnoInspecaoEntrada)
}

func (service *EstoqueLocalPedidoVendaService) AtualizarDistribuicaoInspecaoEstoquePedidoVenda(id uuid.UUID, recnoInspecaoEntrada int,
	input dto.EstoqueLocalPedidoVendaAlocacaoInput) (*dto.ValidacaoDTO, error) {
	alocacao, err := service.EstoquePedidoVendaRepository.BuscarAlocacaoEstoquePedidoVenda(id)
	if err != nil {
		return nil, err
	}

	quantidadesAlocadasPedido, err := service.EstoquePedidoVendaRepository.BuscarQuantidadeTotalAlocadaPedidoVenda(recnoInspecaoEntrada)
	if err != nil {
		return nil, err
	}

	quantidadeTotalAlocada := utils.Round(quantidadesAlocadasPedido.QuantidadeTotalAlocada-
		(utils.DecimalToFloat64(*alocacao.QuantidadeAprovar)+utils.DecimalToFloat64(*alocacao.QuantidadeReprovar))+
		(input.QuantidadeAprovada+input.QuantidadeReprovada), .5, 6)

	inspecao, err := service.InspecaoEntradaRepository.BuscarInspecaoEntradaPeloRecno(recnoInspecaoEntrada)
	if err != nil {
		return nil, err
	}

	if utils.DecimalToFloat64(inspecao.QuantidadeLote) < quantidadeTotalAlocada {
		return &dto.ValidacaoDTO{
			Code:    27,
			Message: "Atenção a somatória das quantidades de inspeção dos pedidos ultrapassa a quantidade de inspeção!",
		}, nil
	}

	somaQuantidadesLotes := 0.0

	for _, item := range input.Lotes {
		somaQuantidadesLotes += item.Quantidade
	}

	if somaQuantidadesLotes > input.QuantidadeAprovada {
		return &dto.ValidacaoDTO{
			Code:    27,
			Message: "A quantidade quebrada em lotes não pode ser maior que a quantidade aprovada",
		}, nil
	}
	var idsToFilter = make([]uuid.UUID, 0)
	idsToFilter = append(idsToFilter, alocacao.Id)
	lotesInseridos, err := service.InspecaoEntradaPedidoVendaLoteRepository.GetAllByIdPedidoVenda(idsToFilter)

	idsLotesToDelete := make([]*uuid.UUID, 0)
	for _, lote := range lotesInseridos {
		var idAsUUID, _ = uuid.Parse(lote.Id)

		idsLotesToDelete = append(idsLotesToDelete, &idAsUUID)
	}

	_ = service.Uow.Begin()
	defer service.Uow.UnitOfWorkGuard()

	err = service.InspecaoEntradaPedidoVendaLoteRepository.BatchDelete(idsLotesToDelete)
	if err != nil {
		_ = service.Uow.Rollback()
		return nil, err
	}

	for _, loteDto := range input.Lotes {
		var lote = entities.InspecaoEntradaPedidoVendaLote{
			Id:                           loteDto.Id.String(),
			IdInspecaoEntradaPedidoVenda: loteDto.IdInspecaoEntradaPedidoVenda,
			NumeroLote:                   loteDto.NumeroLote,
			Quantidade:                   loteDto.Quantidade,
		}
		err = service.InspecaoEntradaPedidoVendaLoteRepository.Create(lote)
		if err != nil {
			_ = service.Uow.Rollback()
			return nil, err
		}
	}

	err = service.EstoquePedidoVendaRepository.AtualizarDistribuicaoInspecaoEstoquePedidoVenda(id, input)

	if err != nil {
		_ = service.Uow.Rollback()
		return nil, err
	}
	_ = service.Uow.Complete()

	return nil, nil
}

func (service *EstoqueLocalPedidoVendaService) BuscarEstoqueLocalPedidosVendas(filter *models.BaseFilter, recnoInspecao int, lote, codigoProduto string) (*dto.GetAllEstoqueLocalPedidoVendaAlocacaoDTO, error) {
	localReprovado, err := service.LocaisRepository.BuscarLocalReprovado()
	if err != nil {
		return nil, err
	}

	localAprovado, err := service.LocaisRepository.BuscarLocalAprovado(codigoProduto)
	if err != nil {
		return nil, err
	}

	err = service.EstoquePedidoVendaRepository.SeedEstoqueLocalPedidosVendasInspecaoValues(localAprovado, localReprovado, recnoInspecao, lote, codigoProduto)
	if err != nil {
		return nil, err
	}

	totalCount, err := service.EstoquePedidoVendaRepository.BuscarTotalCountEstoqueLocalPedidosVendas(recnoInspecao, lote, codigoProduto)
	if err != nil {
		return nil, err
	}

	items, err := service.EstoquePedidoVendaRepository.BuscarEstoqueLocalPedidosVendas(filter, recnoInspecao, lote, codigoProduto)
	if err != nil {
		return nil, err
	}

	return &dto.GetAllEstoqueLocalPedidoVendaAlocacaoDTO{
		Items:      items,
		TotalCount: totalCount,
	}, nil
}

func (service *EstoqueLocalPedidoVendaService) BuscarPedidoVendaLotes(idPedidoVenda uuid.UUID) (*dto.GetAllPedidoVendaLotes, error) {
	var idsToFilter = make([]uuid.UUID, 0)
	idsToFilter = append(idsToFilter, idPedidoVenda)
	items, err := service.InspecaoEntradaPedidoVendaLoteRepository.GetAllByIdPedidoVenda(idsToFilter)
	if err != nil {
		return nil, err
	}
	var itensDto = make([]dto.PedidoVendaLoteDto, 0)

	for _, item := range items {
		var idAsUUID, _ = uuid.Parse(item.Id)

		itensDto = append(itensDto, dto.PedidoVendaLoteDto{
			Id:                           idAsUUID,
			IdInspecaoEntradaPedidoVenda: item.IdInspecaoEntradaPedidoVenda,
			NumeroLote:                   item.NumeroLote,
			Quantidade:                   item.Quantidade,
		})
	}

	totalCount, err := service.InspecaoEntradaPedidoVendaLoteRepository.GetTotalCount(idPedidoVenda)
	if err != nil {

		return nil, err
	}

	return &dto.GetAllPedidoVendaLotes{
		Items:      itensDto,
		TotalCount: totalCount,
	}, nil
}
