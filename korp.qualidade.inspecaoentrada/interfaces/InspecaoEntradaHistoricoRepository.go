package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoEntradaHistoricoRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IInspecaoEntradaHistoricoRepository
type IInspecaoEntradaHistoricoRepository interface {
	BuscarNotasFiscaisHistorico(baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) ([]models.NotaFiscalModel, error)
	BuscarNotasFiscaisHistoricoTotalCount(filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) (int64, error)
	BuscarInspecoesEntradaHistorico(notaFiscal int, lote string, filter *models.BaseFilter) ([]dto.InspecaoEntradaHistoricoItems, error)
	BuscarQuantidadeInspecoesEntradaHistorico(notaFiscal int, lote string) (int64, error)
}
