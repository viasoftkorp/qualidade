package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"github.com/google/uuid"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoEntradaPedidoVendaLoteRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IInspecaoEntradaPedidoVendaLoteRepository
type IInspecaoEntradaPedidoVendaLoteRepository interface {
	Create(input entities.InspecaoEntradaPedidoVendaLote) error
	BatchDelete(ids []*uuid.UUID) error
	GetAllByIdPedidoVenda(idsPedidoVenda []uuid.UUID) ([]*entities.InspecaoEntradaPedidoVendaLote, error)
	GetTotalCount(idPedidoVenda uuid.UUID) (int64, error)
}
