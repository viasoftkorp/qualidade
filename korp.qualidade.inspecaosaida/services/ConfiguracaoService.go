package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
)

type ConfiguracaoService struct {
	interfaces.IConfiguracaoService
	ConfiguracaoRepository interfaces.IConfiguracaoRepository
}

func NewConfiguracaoService(configuracaoRepository interfaces.IConfiguracaoRepository) interfaces.IConfiguracaoService {
	return &ConfiguracaoService{
		ConfiguracaoRepository: configuracaoRepository,
	}
}

func (service *ConfiguracaoService) GetConfiguracao() (dto.ConfiguracaoDto, error) {
	configuracao, err := service.ConfiguracaoRepository.GetConfiguracao()
	return configuracao, err
}

func (service *ConfiguracaoService) UpdateConfiguracao(configuracao dto.ConfiguracaoDto) error {
	err := service.ConfiguracaoRepository.UpdateConfiguracao(configuracao)
	return err
}
