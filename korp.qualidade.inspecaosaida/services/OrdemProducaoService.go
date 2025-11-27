package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/mappers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

type OrdemProducaoService struct {
	interfaces.IOrdemProducaoService
	OrdemProducaoRepository interfaces.IOrdemProducaoRepository
}

func NewOrdemProducaoService(ordemProducaoRepository interfaces.IOrdemProducaoRepository) interfaces.IOrdemProducaoService {
	return &OrdemProducaoService{
		OrdemProducaoRepository: ordemProducaoRepository,
	}
}

func (service *OrdemProducaoService) BuscarOrdensInspecao(baseFilters *models.BaseFilter, filters *dto.OrdemProducaoFilters) (*dto.GetOrdemProducaoDTO, *dto.ValidacaoDTO) {
	ordens, err := service.OrdemProducaoRepository.BuscarOrdensInspecao(baseFilters, filters)
	if err != nil {
		return nil, &dto.ValidacaoDTO{
			Code:    1,
			Message: err.Error(),
		}
	}

	return &dto.GetOrdemProducaoDTO{
		Items:      mappers.MapOrdemProducaoEntitiesToDTOs(ordens),
		TotalCount: int64(0),
	}, nil
}
