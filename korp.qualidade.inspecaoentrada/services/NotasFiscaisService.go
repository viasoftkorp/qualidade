package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/mappers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

type NotaFiscalService struct {
	interfaces.INotaFiscalService
	NotaFiscalRepository interfaces.INotaFiscalRepository
}

func NewNotaFiscalService(notaFiscalRepository interfaces.INotaFiscalRepository) interfaces.INotaFiscalService {
	return &NotaFiscalService{
		NotaFiscalRepository: notaFiscalRepository,
	}
}

func (service *NotaFiscalService) BuscarNotasFiscais(filter *models.BaseFilter, filters *dto.NotaFiscalFilters) (*dto.GetNotasFiscaisOutput, *dto.ValidacaoDTO, error) {
	notasFiscais, err := service.NotaFiscalRepository.BuscarNotasFiscais(filter, filters)
	if err != nil {
		return nil, nil, err
	}

	totalCount, err := service.NotaFiscalRepository.BuscarNotasFiscaisTotalCount(filters)
	if err != nil {
		return nil, nil, err
	}

	notasFiscaisDto := mappers.MapNotasModelToDTO(notasFiscais)

	output := &dto.GetNotasFiscaisOutput{
		notasFiscaisDto,
		totalCount,
	}

	return output, nil, nil
}
