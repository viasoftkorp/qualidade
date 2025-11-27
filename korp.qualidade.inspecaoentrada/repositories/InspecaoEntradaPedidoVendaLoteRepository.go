package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"context"
	"github.com/google/uuid"
	"time"
)

type InspecaoEntradaPedidoVendaLoteRepository struct {
	interfaces.IInspecaoEntradaPedidoVendaLoteRepository
	Uow unit_of_work.UnitOfWork
}

func NewInspecaoEntradaPedidoVendaLoteRepository(uow unit_of_work.UnitOfWork) (interfaces.IInspecaoEntradaPedidoVendaLoteRepository, error) {
	return &InspecaoEntradaPedidoVendaLoteRepository{
		Uow: uow,
	}, nil
}

func (repo *InspecaoEntradaPedidoVendaLoteRepository) Create(input entities.InspecaoEntradaPedidoVendaLote) error {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	updateQueryResult := repo.Uow.GetDb().WithContext(ctx).
		Create(&input)
	return updateQueryResult.Error
}

func (repo *InspecaoEntradaPedidoVendaLoteRepository) GetAllByIdPedidoVenda(idsPedidoVenda []uuid.UUID) ([]*entities.InspecaoEntradaPedidoVendaLote, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	var result []*entities.InspecaoEntradaPedidoVendaLote

	queryResult := repo.Uow.GetDb().WithContext(ctx).
		Raw(queries.GetAllPedidoVendaLoteQuery, idsPedidoVenda).
		Scan(&result)

	return result, queryResult.Error
}

func (repo *InspecaoEntradaPedidoVendaLoteRepository) GetTotalCount(idPedidoVenda uuid.UUID) (int64, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	var result int64

	queryResult := repo.Uow.GetDb().WithContext(ctx).
		Model(&entities.InspecaoEntradaPedidoVendaLote{}).
		Where("ID_INSPECAO_ENTRADA_PEDIDO_VENDA = ?", idPedidoVenda).
		Count(&result)

	return result, queryResult.Error
}

func (repo *InspecaoEntradaPedidoVendaLoteRepository) BatchDelete(ids []*uuid.UUID) error {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	updateQueryResult := repo.Uow.GetDb().WithContext(ctx).
		Where("id IN ?", ids).
		Delete(&entities.InspecaoEntradaPedidoVendaLote{})
	return updateQueryResult.Error
}
