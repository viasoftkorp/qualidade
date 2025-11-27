package mappers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
)

func MapInspecaoEntradaEntitiesToDTOs(entities []entities.InspecaoEntrada) []dto.InspecaoEntradaDTO {
	var dtos []dto.InspecaoEntradaDTO

	for _, entity := range entities {
		dtos = append(dtos, MapInspecaoEntradaEntityToDTO(entity))
	}

	return dtos
}

func MapInspecaoEntradaEntityToDTO(entity entities.InspecaoEntrada) dto.InspecaoEntradaDTO {
	return dto.InspecaoEntradaDTO{
		RecnoInspecao:       entity.Recno,
		CodigoInspecao:      entity.CodigoInspecao,
		NotaFiscal:          entity.NotaFiscal,
		DataInspecao:        utils.StringToTime(entity.DataInspecao),
		Inspetor:            entity.Inspetor,
		Resultado:           entity.Resultado,
		QuantidadeInspecao:  utils.DecimalToFloat64(entity.QuantidadeInspecao),
		QuantidadeLote:      utils.DecimalToFloat64(entity.QuantidadeLote),
		QuantidadeAceita:    utils.DecimalToFloat64(entity.QuantidadeAceita),
		QuantidadeAprovada:  utils.DecimalToFloat64(entity.QuantidadeAprovada),
		QuantidadeReprovada: utils.DecimalToFloat64(entity.QuantidadeReprovada),
	}
}

func MapPlanosInspecaoModelsToDTOs(models []models.PlanoInspecao) []*dto.PlanoInspecaoDTO {
	var dtos []*dto.PlanoInspecaoDTO

	for _, model := range models {
		dtos = append(dtos, MapPlanoInspecaoModelToDTO(model))
	}

	return dtos
}

func MapPlanoInspecaoModelToDTO(model models.PlanoInspecao) *dto.PlanoInspecaoDTO {
	return &dto.PlanoInspecaoDTO{
		Id:                     model.Id.String(),
		LegacyId:               model.LegacyId,
		Descricao:              model.Descricao,
		Resultado:              model.Resultado,
		MaiorValorInspecionado: model.MaiorValorInspecionado,
		MenorValorInspecionado: model.MenorValorInspecionado,
		MaiorValorBase:         model.MaiorValorBase,
		MenorValorBase:         model.MenorValorBase,
		Metodo:                 model.Metodo,
	}
}

func MapInspecaoEntradaItemModelsToDTOs(models []models.InspecaoEntradaItem) []dto.InspecaoEntradaItemDTO {
	var dtos []dto.InspecaoEntradaItemDTO

	for _, model := range models {
		dtos = append(dtos, mapInspecaoEntradaItemModelToDTO(model))
	}

	return dtos
}

func mapInspecaoEntradaItemModelToDTO(model models.InspecaoEntradaItem) dto.InspecaoEntradaItemDTO {
	return dto.InspecaoEntradaItemDTO{
		Id:                     model.Id.String(),
		LegacyIdPlanoInspecao:  model.LegacyIdPlanoInspecao,
		CodigoPlano:            model.Plano,
		Descricao:              model.Descricao,
		Metodo:                 model.Metodo,
		Sequencia:              model.Sequencia,
		Resultado:              model.Resultado,
		MaiorValorInspecionado: utils.DecimalToFloat64(model.MaiorValorInspecionado),
		MenorValorInspecionado: utils.DecimalToFloat64(model.MenorValorInspecionado),
		MaiorValorBase:         utils.DecimalToFloat64(model.MaiorValorBase),
		MenorValorBase:         utils.DecimalToFloat64(model.MenorValorBase),
		Observacao:             model.Observacao,
	}
}
