package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_ILocaisRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces ILocaisRepository
type ILocaisRepository interface {
	BuscarLocalAprovado(codigoProduto string) (int, error)
	BuscarLocalReprovado() (int, error)
	BuscarLocalEntrada() (int, error)
	BuscarLocalDescricao(codigoLocal int) (string, error)
	BuscarLocaisPrincipais(codigoProduto string) ([]dto.LocalOutput, error)
	BuscarLocaisReprovados() ([]dto.LocalOutput, error)
	BuscarLocalPeloCodigo(codigoLocal int) (*dto.LocalOutput, error)
	BuscarLocais(filterInput *models.BaseFilter) ([]dto.LocalOutput, error)
	BuscarLocaisTotalCount(filterInput *models.BaseFilter) (int64, error)
}
