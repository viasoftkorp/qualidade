package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"github.com/google/uuid"
)

//go:generate mockgen -destination=../mocks/mock_EstoquePedidoVendaRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IEstoquePedidoVendaRepository
type IEstoquePedidoVendaRepository interface {
	SeedEstoqueLocalPedidosVendasInspecaoValues(localAprovado, localReprovado, recnoInspecao int, lote, codigoProduto string) error
	RemoverInspecaoEntradaPedidoVenda(recnoInspecao int) error
	BuscarTotalCountEstoqueLocalPedidosVendas(recnoInspecao int, lote, codigoProduto string) (int64, error)
	BuscarEstoqueLocalPedidosVendas(filter *models.BaseFilter, recnoInspecao int, lote, codigoProduto string) ([]dto.EstoqueLocalPedidoVendaAlocacaoDTO, error)
	BuscarQuantidadeTotalAlocadaPedidoVenda(recnoInspecaoEntrada int) (*dto.EstoqueLocalPedidoVendoTotalizacaoDTO, error)
	AtualizarDistribuicaoInspecaoEstoquePedidoVenda(id uuid.UUID, input dto.EstoqueLocalPedidoVendaAlocacaoInput) error
	BuscarAlocacaoEstoquePedidoVenda(id uuid.UUID) (entities.InspecaoEntradaPedidoVenda, error)
	BuscarEstoqueLocalValoresPorProduto(codigoProduto string, lote string, codigoLocal int) (*models.EstoqueLocalValores, error)
	BuscarPacotes(recnoEstoqueLocal int) ([]models.Pacote, error)
	BuscarSeries(recnoEstoqueLocal int) ([]models.Serie, error)
}
