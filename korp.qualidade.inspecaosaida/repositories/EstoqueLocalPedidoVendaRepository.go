package repositories

import (
	"database/sql"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
)

type EstoqueLocalPedidoVendaRepository struct {
	interfaces.IEstoquePedidoVendaRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewEstoqueLocalPedidoVendaRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (interfaces.IEstoquePedidoVendaRepository, error) {
	return &EstoqueLocalPedidoVendaRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *EstoqueLocalPedidoVendaRepository) BuscarEstoqueLocalValoresPorProduto(codigoProduto string, lote string, odf int, codigoLocal int) (*models.EstoqueLocalValores, error) {
	var estoqueLocalValores *models.EstoqueLocalValores

	itemsQueryResult := repo.Uow.GetDb().Raw(queries.BuscarEstoqueLocalValoresPorProduto,
		sql.Named(queries.NamedProdutoCodigo, codigoProduto),
		sql.Named(queries.NamedLote, lote),
		sql.Named(queries.NamedOdf, odf),
		sql.Named(queries.NamedLocal, codigoLocal),
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId)).
		Scan(&estoqueLocalValores)

	if itemsQueryResult.Error != nil {
		return nil, itemsQueryResult.Error
	}

	return estoqueLocalValores, nil
}

func (repo *EstoqueLocalPedidoVendaRepository) BuscarPacotes(recnoEstoqueLocal int) ([]models.Pacote, error) {
	pacotes := make([]models.Pacote, 0)

	itemsQueryResult := repo.Uow.GetDb().Raw(queries.BuscarPacotes,
		sql.Named(queries.NamedLocal, recnoEstoqueLocal)).
		Scan(&pacotes)

	if itemsQueryResult.Error != nil {
		return nil, itemsQueryResult.Error
	}

	return pacotes, nil
}

func (repo *EstoqueLocalPedidoVendaRepository) BuscarSeries(recnoEstoqueLocal int) ([]models.Serie, error) {
	series := make([]models.Serie, 0)

	itemsQueryResult := repo.Uow.GetDb().Raw(queries.BuscarSeries,
		sql.Named(queries.NamedLocal, recnoEstoqueLocal)).
		Scan(&series)

	if itemsQueryResult.Error != nil {
		return nil, itemsQueryResult.Error
	}

	return series, nil
}
