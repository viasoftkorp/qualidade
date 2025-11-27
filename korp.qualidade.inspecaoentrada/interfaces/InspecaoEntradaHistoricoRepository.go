package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoEntradaHistoricoRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IInspecaoEntradaHistoricoRepository
type IInspecaoEntradaHistoricoRepository interface {
	BuscarNotasFiscaisHistorico(baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) ([]models.NotaFiscalModel, error)
	BuscarNotasFiscaisHistoricoTotalCount(baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) (int64, error)
	BuscarInspecoesEntradaHistorico(recnoItemNotaFiscal int, notaFiscal int, lote string, baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) ([]dto.InspecaoEntradaHistoricoItems, error)
	BuscarQuantidadeInspecoesEntradaHistorico(recnoItemNotaFiscal int, notaFiscal int, lote string, baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) (int64, error)
}
