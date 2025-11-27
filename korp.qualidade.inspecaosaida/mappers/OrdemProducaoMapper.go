package mappers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
)

func MapOrdemProducaoEntitiesToDTOs(entities []models.OrdemProducao) []dto.OrdemProducaoDTO {
	var dtos []dto.OrdemProducaoDTO

	for _, entity := range entities {
		dtos = append(dtos, mapOrdemProducaoEntityToDTO(entity))
	}

	return dtos
}

func mapOrdemProducaoEntityToDTO(entity models.OrdemProducao) dto.OrdemProducaoDTO {
	return dto.OrdemProducaoDTO{
		ODF:                    entity.ODF,
		CodigoProduto:          entity.CodigoProduto,
		DescricaoProduto:       entity.DescricaoProduto,
		Situacao:               entity.Situacao,
		Revisao:                entity.Revisao,
		QuantidadeOrdem:        utils.DecimalToFloat64(entity.QuantidadeOrdem),
		QuantidadeProduzida:    utils.DecimalToFloat64(entity.QuantidadeProduzida),
		DataInicio:             utils.StringToTime(entity.DataInicio),
		DataEntrega:            utils.StringToTime(entity.DataEntrega),
		DataEmissao:            utils.StringToTime(entity.DataEmissao),
		Lote:                   entity.Lote,
		QuantidadeInspecionada: utils.DecimalToFloat64(entity.QuantidadeInspecionada),
		QuantidadeInspecionar:  utils.DecimalToFloat64(entity.QuantidadeProduzida.Sub(entity.QuantidadeInspecionada)),
		Plano:                  entity.Plano,
		DataNegociada:          entity.DataNegociada,
		DescricaoPlano:         entity.DescricaoPlano,
		NumeroPedido:           entity.NumeroPedido,
		Cliente:                entity.Cliente,
		OdfApontada:            entity.OdfApontada,
		RecnoProcesso:          entity.RecnoProcesso,
	}
}
