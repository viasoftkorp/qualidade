package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoSaidaHistoricoService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IInspecaoSaidaHistoricoService
type IInspecaoSaidaHistoricoService interface {
	GetAllInspecaoSaidaHistoricoCabecalho(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters) (*dto.GetAllInspecaoSaidaHistoricoCabecalhoDTO, error)
	GetAllInspecaoSaidaHistoricoItems(baseFilters *models.BaseFilter, odf int) (*dto.GetAllInspecaoSaidaHistoricoItemsDTO, error)
}
