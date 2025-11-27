package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_ProdutoRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IProdutoRepository
type IProdutoRepository interface {
	BuscarProdutoDescricao(codigoProduto string) (string, error)
	BuscarProdutoPeloCodigo(codigoProduto string) (*dto.ProdutoOutput, error)
	BuscarProdutos(filterInput *models.BaseFilter) ([]dto.ProdutoOutput, error)
	BuscarProdutosTotalCount(filterInput *models.BaseFilter) (int64, error)
}
