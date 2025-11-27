package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_LocalService.go -package=mocks bitbucket.org/viasoftkorp/korp.logistica.estoque.ajusteestoque/interfaces ILocalService

type ILocalService interface {
	BuscarLocal(codigo int) (*dto.LocalOutput, error)
	BuscarLocais(filterInput *models.BaseFilter) (*dto.GetLocais, error)
}
