package mappers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
)

func MapInspecaoSaidaEntitiesToDTOs(entities []*entities.InspecaoSaida) []*dto.InspecaoSaidaDTO {
	var dtos []*dto.InspecaoSaidaDTO

	for _, entity := range entities {
		dtos = append(dtos, MapInspecaoSaidaEntityToDTO(entity))
	}

	return dtos
}

func MapInspecaoSaidaEntityToDTO(entity *entities.InspecaoSaida) *dto.InspecaoSaidaDTO {
	return &dto.InspecaoSaidaDTO{
		CodigoInspecao:         entity.CodigoInspecao,
		ODF:                    entity.Odf,
		DataInspecao:           utils.StringToTime(entity.DataInspecao),
		Inspetor:               entity.Inspetor,
		Resultado:              entity.Resultado,
		QuantidadeInspecao:     utils.DecimalToFloat64(entity.QuantidadeInspecao),
		QuantidadeLote:         utils.DecimalToFloat64(entity.QuantidadeLote),
		QuantidadeAceita:       utils.DecimalToFloat64(entity.QuantidadeAceita),
		QuantidadeRetrabalhada: utils.DecimalToFloat64(entity.QuantidadeRetrabalhada),
		QuantidadeAprovada:     utils.DecimalToFloat64(entity.QuantidadeAprovada),
		QuantidadeReprovada:    utils.DecimalToFloat64(entity.QuantidadeReprovada),
		NumeroPedido:           entity.Pedido,
		Cliente:                entity.Cliente,
		TipoInspecao:           entity.TipoInspecao,
	}
}

func MapInspecaoSaidaDetalhesToDTO(model *models.InspecaoSaida) *dto.InspecaoSaidaDTO {
	return &dto.InspecaoSaidaDTO{
		CodigoInspecao:         model.CodigoInspecao,
		ODF:                    model.Odf,
		DataInspecao:           utils.StringToTime(model.DataInspecao),
		Inspetor:               model.Inspetor,
		Resultado:              model.Resultado,
		QuantidadeInspecao:     utils.DecimalToFloat64(model.QtdInspecao),
		QuantidadeLote:         utils.DecimalToFloat64(model.QtdLote),
		QuantidadeAceita:       utils.DecimalToFloat64(model.QuantidadeAceita),
		QuantidadeRetrabalhada: utils.DecimalToFloat64(model.QuantidadeRetrabalhada),
		QuantidadeAprovada:     utils.DecimalToFloat64(model.QuantidadeAprovada),
		QuantidadeReprovada:    utils.DecimalToFloat64(model.QuantidadeReprovada),
		Lote:                   model.Lote,
		RecnoInspecaoSaida:     model.Recno,
		CodigoProduto:          model.CodigoProduto,
		QuantidadeOrdem:        utils.DecimalToFloat64(model.QuantidadeOrdem),
		NumeroPedido:           model.NumeroPedido,
	}
}

func MapPlanosInspecaoModelsToDTOs(models []*models.PlanoInspecao) []*dto.PlanoInspecaoDTO {
	var dtos []*dto.PlanoInspecaoDTO

	for _, model := range models {
		dtos = append(dtos, MapPlanoInspecaoModelToDTO(model))
	}

	return dtos
}

func MapPlanoInspecaoModelToDTO(model *models.PlanoInspecao) *dto.PlanoInspecaoDTO {
	return &dto.PlanoInspecaoDTO{
		Id:             model.Id.String(),
		Descricao:      model.Descricao,
		Resultado:      model.Resultado,
		MaiorValor:     model.MaiorValor,
		MenorValor:     model.MenorValor,
		MaiorValorBase: model.MaiorValorBase,
		MenorValorBase: model.MenorValorBase,
		Metodo:         model.Metodo,
	}
}

func MapInspecaoSaidaItemModelsToDTOs(models []*models.InspecaoSaidaItem) []*dto.InspecaoSaidaItemDTO {
	var dtos []*dto.InspecaoSaidaItemDTO

	for _, model := range models {
		dtos = append(dtos, mapInspecaoSaidaItemModelToDTO(model))
	}

	return dtos
}

func mapInspecaoSaidaItemModelToDTO(model *models.InspecaoSaidaItem) *dto.InspecaoSaidaItemDTO {
	return &dto.InspecaoSaidaItemDTO{
		Id:             model.Id.String(),
		Plano:          model.Plano,
		Odf:            model.Odf,
		Descricao:      model.Descricao,
		Metodo:         model.Metodo,
		Sequencia:      model.Sequencia,
		Resultado:      model.Resultado,
		MaiorValor:     utils.DecimalToFloat64(model.MaiorValor),
		MenorValor:     utils.DecimalToFloat64(model.MenorValor),
		MaiorValorBase: utils.DecimalToFloat64(model.MaiorValorBase),
		MenorValorBase: utils.DecimalToFloat64(model.MenorValorBase),
		Observacao:     model.Observacao,
	}
}
