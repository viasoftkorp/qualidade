package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_LocaisRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces ILocaisRepository
type ILocaisRepository interface {
	BuscarLocalRetrabalho() (int, error)
	BuscarLocalReprovado() (int, error)
	BuscarLocalAprovado() (int, error)
	BuscarLocalSaida() (int, error)
	BuscarLocalDescricao(codigoLocal int) (string, error)
	BuscarLocaisRetrabalho() ([]map[string]interface{}, error)
	BuscarLocaisReprovados() ([]map[string]interface{}, error)
	BuscarLocaisAprovados() ([]map[string]interface{}, error)
	BuscarLocalPeloCodigo(codigoLocal int) (*dto.LocalOutput, error)
	BuscarLocais(filterInput *models.BaseFilter) ([]dto.LocalOutput, error)
	BuscarLocaisTotalCount(filterInput *models.BaseFilter) (int64, error)
}
