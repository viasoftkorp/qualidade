package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

type LocalService struct {
	interfaces.ILocalService
	LocalRepository interfaces.ILocaisRepository
}

func NewLocalService(localRepository interfaces.ILocaisRepository) interfaces.ILocalService {
	return &LocalService{
		LocalRepository: localRepository,
	}
}
func (service *LocalService) BuscarLocal(codigo int) (*dto.LocalOutput, error) {
	local, err := service.LocalRepository.BuscarLocalPeloCodigo(codigo)
	if err != nil {
		return nil, err
	}

	return local, nil
}

func (service *LocalService) BuscarLocais(filter *models.BaseFilter) (*dto.GetLocais, error) {
	locais, err := service.LocalRepository.BuscarLocais(filter)
	if err != nil {
		return nil, err
	}

	count, err := service.LocalRepository.BuscarLocaisTotalCount(filter)
	if err != nil {
		return nil, err
	}

	getLocais := dto.GetLocais{
		Items:      locais,
		TotalCount: count,
	}

	return &getLocais, nil
}
