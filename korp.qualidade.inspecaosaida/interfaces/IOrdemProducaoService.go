package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_OrdemProducaoService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IOrdemProducaoService
type IOrdemProducaoService interface {
	BuscarOrdensInspecao(baseFilters *models.BaseFilter, filters *dto.OrdemProducaoFilters) (*dto.GetOrdemProducaoDTO, *dto.ValidacaoDTO)
}
