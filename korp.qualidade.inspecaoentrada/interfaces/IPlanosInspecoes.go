package interfaces

import "bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"

//go:generate mockgen -destination=../mocks/mock_PlanosInspecaoRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IPlanosInspecaoRepository
type IPlanosInspecaoRepository interface {
	BuscarPlanosNovaInspecao(plano string, codigoProduto string, filter *models.BaseFilter) ([]models.PlanoInspecao, error)
	BuscarQuantidadePlanosNovaInspecao(plano string, codigoProduto string) (int64, error)
	BuscarTodosPlanosNotaFiscalProduto(plano string, codigoProduto string) ([]models.PlanoInspecao, error)
}
