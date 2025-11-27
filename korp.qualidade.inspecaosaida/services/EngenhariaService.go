package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
)

type EngenhariaService struct {
	interfaces.IEngenhariaService
	EngenhariaRepository interfaces.IEngenhariaRepository
}

func NewEngenhariaService(EngenhariaRepository interfaces.IEngenhariaRepository) interfaces.IEngenhariaService {
	return &EngenhariaService{
		EngenhariaRepository: EngenhariaRepository,
	}
}
func (service *EngenhariaService) BuscarProcesso(codigoProduto string) (*dto.ProcessoEngenhariaOutput, error) {
	processo, err := service.EngenhariaRepository.BuscarProcesso(codigoProduto)
	if err != nil {
		return nil, err
	}

	return processo, nil
}
