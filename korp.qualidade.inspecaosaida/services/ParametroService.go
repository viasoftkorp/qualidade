package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
)

type ParametroService struct {
	interfaces.IParametroService
	ParametroRepository interfaces.IParametroRepository
}

func NewParametroService(parametroRepository interfaces.IParametroRepository) interfaces.IParametroService {
	return &ParametroService{
		ParametroRepository: parametroRepository,
	}
}

func (service *ParametroService) BuscarParametroBool(parametro, secao string) (bool, error) {
	return service.ParametroRepository.BuscarParametroBool(parametro, secao)
}
