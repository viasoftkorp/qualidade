package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
)

//go:generate mockgen -destination=../mocks/mock_LocalService.go -package=mocks bitbucket.org/viasoftkorp/korp.logistica.estoque.ajusteestoque/interfaces IConfiguracaoService

type IConfiguracaoService interface {
	GetConfiguracao() (dto.ConfiguracaoDto, error)
	UpdateConfiguracao(dto.ConfiguracaoDto) error
}
