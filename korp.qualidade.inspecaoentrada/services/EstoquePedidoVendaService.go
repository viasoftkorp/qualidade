package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	"github.com/google/uuid"
)

type EstoqueLocalPedidoVendaService struct {
	interfaces.IEstoquePedidoVendaService
	EstoquePedidoVendaRepository interfaces.IEstoquePedidoVendaRepository
	LocaisRepository             interfaces.ILocaisRepository
	InspecaoEntradaRepository    interfaces.IInspecaoEntradaRepository
}

func NewEstoqueLocalPedidoVendaService(estoquePedidoVendaRepository interfaces.IEstoquePedidoVendaRepository,
	locaisRepository interfaces.ILocaisRepository,
	inspecaoEntradaRepository interfaces.IInspecaoEntradaRepository) interfaces.IEstoquePedidoVendaService {
	return &EstoqueLocalPedidoVendaService{
		EstoquePedidoVendaRepository: estoquePedidoVendaRepository,
		LocaisRepository:             locaisRepository,
		InspecaoEntradaRepository:    inspecaoEntradaRepository,
	}
}

func (service *EstoqueLocalPedidoVendaService) BuscarQuantidadeTotalAlocadaPedidoVenda(recnoInspecaoEntrada int) (*dto.EstoqueLocalPedidoVendoTotalizacaoDTO, error) {
	return service.EstoquePedidoVendaRepository.BuscarQuantidadeTotalAlocadaPedidoVenda(recnoInspecaoEntrada)
}

func (service *EstoqueLocalPedidoVendaService) AtualizarDistribuicaoInspecaoEstoquePedidoVenda(id uuid.UUID, recnoInspecaoEntrada int, input dto.EstoqueLocalPedidoVendaAlocacaoDTO) (*dto.ValidacaoDTO, error) {
	alocacao, err := service.EstoquePedidoVendaRepository.BuscarAlocacaoEstoquePedidoVenda(id)
	if err != nil {
		return nil, err
	}

	quantidadesAlocadasPedido, err := service.EstoquePedidoVendaRepository.BuscarQuantidadeTotalAlocadaPedidoVenda(recnoInspecaoEntrada)
	if err != nil {
		return nil, err
	}

	quantidadeTotalAlocada := quantidadesAlocadasPedido.QuantidadeTotalAlocada -
		(utils.DecimalToFloat64(*alocacao.QuantidadeAprovar) + utils.DecimalToFloat64(*alocacao.QuantidadeReprovar)) +
		(input.QuantidadeAprovada + input.QuantidadeReprovada)

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

	return nil, service.EstoquePedidoVendaRepository.AtualizarDistribuicaoInspecaoEstoquePedidoVenda(id, input)
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
