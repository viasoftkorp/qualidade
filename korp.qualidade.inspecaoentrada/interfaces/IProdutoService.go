package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_ProdutoService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IProdutoService

type IProdutoService interface {
	BuscarProduto(codigo string) (*dto.ProdutoOutput, error)
	BuscarProdutos(filterInput *models.BaseFilter) (*dto.GetProdutos, error)
}
