package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoSaidaHistoricoRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IInspecaoSaidaHistoricoRepository
type IInspecaoSaidaHistoricoRepository interface {
	GetAllInspecaoSaidaHistoricoCabecalho(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters) ([]dto.InspecaoSaidaHistoricoCabecalhoDTO, error)
	CountInspecaoSaidaHistoricoCabecalho(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters) (int64, error)
	GetAllInspecaoSaidaHistoricoItems(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters, odf int, codigoInspecao int) ([]dto.InspecaoSaidaHistoricoItems, error)
	CountInspecaoSaidaHistoricoItems(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters, odf int, codigoInspecao int) (int64, error)
}
