package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_LocalService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces ILocalService

type ILocalService interface {
	BuscarLocal(codigo int) (*dto.LocalOutput, error)
	BuscarLocais(filterInput *models.BaseFilter) (*dto.GetLocais, error)
}
