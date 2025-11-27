package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"context"
	"database/sql"
	"strconv"
	"time"
)

type NotaFiscalRepository struct {
	interfaces.INotaFiscalRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewNotaFiscalRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (
	interfaces.INotaFiscalRepository, error) {
	return &NotaFiscalRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *NotaFiscalRepository) BuscarNotaFiscal(notaFiscal int, lote string) (models.NotaFiscalModel, error) {
	var result models.NotaFiscalModel

	query := queries.GetNotaFiscal

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
		sql.Named(queries.NamedNotaFiscal, notaFiscal),
		sql.Named(queries.NamedLote, lote)).
		Scan(&result)

	return result, res.Error
}

func (repo *NotaFiscalRepository) BuscarNotasFiscais(filter *models.BaseFilter, filters *dto.NotaFiscalFilters) ([]models.NotaFiscalModel, error) {
	var result []models.NotaFiscalModel

	query := queries.GetNotasFiscaisQuery
	query = repo.AplicarFiltros(query, filters)
	query += queries.GetNotasFiscaisPagination

	var args = models.QueryFilter{
		Skip:     filter.Skip,
		PageSize: filter.PageSize,
	}
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno), args).
		Scan(&result)

	return result, res.Error
}

func (repo *NotaFiscalRepository) BuscarNotasFiscaisTotalCount(filters *dto.NotaFiscalFilters) (int64, error) {
	var result int64

	query := queries.GetNotasFiscaisCount
	query = repo.AplicarFiltros(query, filters)
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno)).Count(&result)

	return result, res.Error
}

func (repo *NotaFiscalRepository) AplicarFiltros(query string, filters *dto.NotaFiscalFilters) string {
	if filters.NotaFiscal != nil {
		query += "AND HISTLISE.NFISCAL = " + strconv.Itoa(*filters.NotaFiscal) + "\n"
	}
	if filters.Lote != nil {
		query += "AND (HISTLISE_RATEIO_LOTE.LOTE LIKE '" + *filters.Lote + "'\n"
		query += "OR HISTLISE.LOTE LIKE '" + *filters.Lote + "')\n"
	}
	if filters.CodigoProduto != nil {
		query += "AND HISTLISE.ITEM LIKE '" + *filters.CodigoProduto + "'\n"
	}
	if filters.Fornecedor != nil {
		query += "AND FORNECED.RASSOC LIKE '" + *filters.Fornecedor + "'\n"
	}
	if filters.DataEntrada != nil {
		dataEntrega := *filters.DataEntrada
		query += "AND CAST(HISTLISE.DTENT AS DATE) = '" + dataEntrega.Format("2006-01-02") + "'\n"
	}

	return query
}
