package mappers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
)

func MapNotaModelToDTO(notaModel *models.NotaFiscalModel) *dto.NotaFiscalDTO {
	if notaModel == nil {
		return nil
	}

	return &dto.NotaFiscalDTO{
		Id:                     notaModel.Id,
		Recno:                  notaModel.Recno,
		NotaFiscal:             notaModel.NotaFiscal,
		Plano:                  notaModel.Plano,
		Lote:                   notaModel.Lote,
		CodigoForneced:         notaModel.CodigoForneced,
		DescricaoForneced:      notaModel.DescricaoForneced,
		CodigoProduto:          notaModel.CodigoProduto,
		DescricaoProduto:       notaModel.DescricaoProduto,
		DataEntrada:            utils.StringToTime(notaModel.DataEntrada),
		Quantidade:             utils.DecimalToFloat64(notaModel.Quantidade),
		QuantidadeInspecionada: utils.DecimalToFloat64(notaModel.QuantidadeInspecionada),
		QuantidadeInspecionar:  utils.DecimalToFloat64(notaModel.Quantidade.Sub(notaModel.QuantidadeInspecionada)),
		RecnoRateioLote:        notaModel.RecnoRateioLote,
		DescricaoPlano:         notaModel.DescricaoPlano,
		Serie:                  notaModel.Serie,
		Observacao:             notaModel.Observacao,
	}
}

func MapNotasModelToDTO(notasModel []models.NotaFiscalModel) []dto.NotaFiscalDTO {
	if notasModel == nil {
		return nil
	}

	var notasDTO []dto.NotaFiscalDTO

	for _, cargaModel := range notasModel {
		notaDTO := MapNotaModelToDTO(&cargaModel)
		notasDTO = append(notasDTO, *notaDTO)
	}

	return notasDTO
}
