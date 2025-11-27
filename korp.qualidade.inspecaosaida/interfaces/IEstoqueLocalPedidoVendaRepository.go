package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_EstoquePedidoVendaRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IEstoquePedidoVendaRepository
type IEstoquePedidoVendaRepository interface {
	BuscarEstoqueLocalValoresPorProduto(codigoProduto string, lote string, odf int, codigoLocal int) (*models.EstoqueLocalValores, error)
	BuscarPacotes(recnoEstoqueLocal int) ([]models.Pacote, error)
	BuscarSeries(recnoEstoqueLocal int) ([]models.Serie, error)
}
