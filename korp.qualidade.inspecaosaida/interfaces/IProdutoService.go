package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_ProdutoService.go -package=mocks bitbucket.org/viasoftkorp/korp.logistica.estoque.ajusteestoque/interfaces IProdutoService

type IProdutoService interface {
	BuscarProduto(codigo string) (*dto.ProdutoOutput, error)
	BuscarProdutos(filterInput *models.BaseFilter) (*dto.GetProdutos, error)
}
