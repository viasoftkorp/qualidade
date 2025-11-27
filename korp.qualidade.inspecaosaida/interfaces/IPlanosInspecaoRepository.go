package interfaces

import "bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"

//go:generate mockgen -destination=../mocks/mock_PlanosInspecaoRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IPlanosInspecaoRepository
type IPlanosInspecaoRepository interface {
	BuscarPlanosNovaInspecao(recnoProcesso int, codProduto string, filter *models.BaseFilter) ([]*models.PlanoInspecao, error)
	BuscarQuantidadePlanosNovaInspecao(recnoProcesso int, codProduto string) (int64, error)
	BuscarTodosPlanosOdfProduto(recnoProcesso int, codProduto string) ([]*models.PlanoInspecao, error)
}
