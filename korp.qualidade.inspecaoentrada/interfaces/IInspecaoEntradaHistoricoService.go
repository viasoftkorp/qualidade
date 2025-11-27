package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoEntradaHistoricoService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IInspecaoEntradaHistoricoService
type IInspecaoEntradaHistoricoService interface {
	GetAllInspecaoEntradaHistoricoCabecalho(baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) (*dto.GetNotasFiscaisOutput, error)
	GetAllInspecaoEntradaHistoricoItems(notaFiscal int, lote string, baseFilters *models.BaseFilter) (*dto.GetAllInspecaoEntradaHistoricoItemsDTO, error)
}
