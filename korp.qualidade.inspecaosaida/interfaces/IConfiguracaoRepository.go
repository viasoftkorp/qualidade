package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
)

//go:generate mockgen -destination=../mocks/mock_LocaisRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IConfiguracaoRepository

type IConfiguracaoRepository interface {
	GetConfiguracao() (dto.ConfiguracaoDto, error)
	UpdateConfiguracao(dto.ConfiguracaoDto) error
}
