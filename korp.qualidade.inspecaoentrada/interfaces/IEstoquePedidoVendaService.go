package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"github.com/google/uuid"
)

//go:generate mockgen -destination=../mocks/mock_EstoquePedidoVendaService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IEstoquePedidoVendaService
type IEstoquePedidoVendaService interface {
	BuscarEstoqueLocalPedidosVendas(filter *models.BaseFilter, recnoInspecao int, lote, codigoProduto string) (*dto.GetAllEstoqueLocalPedidoVendaAlocacaoDTO, error)
	BuscarQuantidadeTotalAlocadaPedidoVenda(recnoInspecaoEntrada int) (*dto.EstoqueLocalPedidoVendoTotalizacaoDTO, error)
	AtualizarDistribuicaoInspecaoEstoquePedidoVenda(id uuid.UUID, recnoInspecaoEntrada int, input dto.EstoqueLocalPedidoVendaAlocacaoDTO) (*dto.ValidacaoDTO, error)
}
