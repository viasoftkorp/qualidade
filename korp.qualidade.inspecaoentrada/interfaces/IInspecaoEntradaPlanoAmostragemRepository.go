package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoEntradaPlanoAmostragemRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IInspecaoEntradaPlanoAmostragemRepository
type IInspecaoEntradaPlanoAmostragemRepository interface {
	GetAll(filter *models.BaseFilter) (*dto.GetAllPlanoAmostragemDTO, error)
	Create(entityToUpdate dto.PlanoAmostragemDTO) error
	Update(entityToUpdate dto.PlanoAmostragemDTO) error
	Delete(id string) error
	GetFaixaPlanoAmostragem(quantidade float64) (*dto.PlanoAmostragemDTO, error)
}
