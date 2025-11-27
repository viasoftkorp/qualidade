package mappers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
)

func MapPedidoVendaModelToDTO(pedidoVendaModel *models.EstoqueLocalPedidoVendaAlocacaoModel) *dto.EstoqueLocalPedidoVendaAlocacaoDTO {
	if pedidoVendaModel == nil {
		return nil
	}

	return &dto.EstoqueLocalPedidoVendaAlocacaoDTO{
		NumeroPedido:               pedidoVendaModel.NumeroPedido,
		QuantidadeTotalPedido:      utils.DecimalToFloat64(pedidoVendaModel.QuantidadeTotalPedido),
		QuantidadeAlocadaLoteLocal: utils.DecimalToFloat64(pedidoVendaModel.QuantidadeAlocadaLoteLocal),
		DescricaoProduto:           pedidoVendaModel.DescricaoProduto,
		DescricaoLocalReprovado:    pedidoVendaModel.DescricaoLocalReprovado,
		DescricaoLocalAprovado:     pedidoVendaModel.DescricaoLocalAprovado,
		CodigoLocalReprovado:       pedidoVendaModel.CodigoLocalReprovado,
		CodigoLocalAprovado:        pedidoVendaModel.CodigoLocalAprovado,
		QuantidadeAprovada:         utils.DecimalToFloat64(pedidoVendaModel.QuantidadeAprovada),
		QuantidadeReprovada:        utils.DecimalToFloat64(pedidoVendaModel.QuantidadeReprovada),
	}
}

func MapPedidosVendasModelstoDTO(pedidosVendaModels []models.EstoqueLocalPedidoVendaAlocacaoModel) []dto.EstoqueLocalPedidoVendaAlocacaoDTO {
	if pedidosVendaModels == nil {
		return nil
	}

	var pedidosVendaDTO []dto.EstoqueLocalPedidoVendaAlocacaoDTO

	for _, cargaModel := range pedidosVendaModels {
		notaDTO := MapPedidoVendaModelToDTO(&cargaModel)
		pedidosVendaDTO = append(pedidosVendaDTO, *notaDTO)
	}

	return pedidosVendaDTO
}
