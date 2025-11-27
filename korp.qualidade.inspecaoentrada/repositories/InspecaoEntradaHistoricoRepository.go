package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"context"
	"database/sql"
	"fmt"
	"strconv"
	"time"
)

type InspecaoEntradaHistoricoRepository struct {
	interfaces.IInspecaoEntradaHistoricoRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewInspecaoEntradaHistoricoRepository(uow unit_of_work.UnitOfWork, baseParams *models.BaseParams) interfaces.IInspecaoEntradaHistoricoRepository {
	return &InspecaoEntradaHistoricoRepository{
		Uow:        uow,
		BaseParams: baseParams,
	}
}

func (repo *InspecaoEntradaHistoricoRepository) BuscarNotasFiscaisHistorico(filter *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) ([]models.NotaFiscalModel, error) {
	var result []models.NotaFiscalModel

	filterQuery := ""
	if filters.NotaFiscal != nil {
		filterQuery += "AND QA_INSPECAO_ENTRADA.CODNOTA = " + strconv.Itoa(*filters.NotaFiscal) + "\n"
	}

	if filters.CodigoProduto != nil {
		filterQuery += "AND HISTLISE.ITEM = " + *filters.CodigoProduto + "\n"
	}

	if filters.Lote != nil {
		filterQuery += "AND QA_INSPECAO_ENTRADA.LOTE = '" + *filters.Lote + "'" + "\n"
	}

	query := fmt.Sprintf(queries.GetNotasFiscaisHistoricoQuery, filterQuery) + queries.GetNotasFiscaisPagination

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

func (repo *InspecaoEntradaHistoricoRepository) BuscarNotasFiscaisHistoricoTotalCount(filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) (int64, error) {
	var result int64

	filterQuery := ""
	if filters.NotaFiscal != nil {
		filterQuery += "AND QA_INSPECAO_ENTRADA.CODNOTA = " + strconv.Itoa(*filters.NotaFiscal) + "\n"
	}

	if filters.CodigoProduto != nil {
		filterQuery += "AND HISTLISE.ITEM = '" + *filters.CodigoProduto + "'" + "\n"
	}

	if filters.Lote != nil {
		filterQuery += "AND QA_INSPECAO_ENTRADA.LOTE = '" + *filters.Lote + "'" + "\n"
	}

	query := fmt.Sprintf(queries.GetNotasFiscaisHistoricoCount, filterQuery)

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno)).Count(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaHistoricoRepository) BuscarInspecoesEntradaHistorico(notaFiscal int, lote string, filter *models.BaseFilter) ([]dto.InspecaoEntradaHistoricoItems, error) {
	var result []dto.InspecaoEntradaHistoricoItems

	query := queries.GetAllInspecaoEntradaItensHistoricoInspecao + queries.GetInspecaoEntradaItensHistoricoInspecaoPagination

	var args = models.QueryFilter{
		Skip:     filter.Skip,
		PageSize: filter.PageSize,
	}

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
		sql.Named(queries.NamedNotaFiscal, notaFiscal),
		sql.Named(queries.NamedLote, lote),
		args).
		Scan(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaHistoricoRepository) BuscarQuantidadeInspecoesEntradaHistorico(notaFiscal int, lote string) (int64, error) {
	var result int64
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Table(entities.InspecaoEntrada{}.TableName()).Where(entities.InspecaoEntrada{
		NotaFiscal: notaFiscal,
		Lote:       lote,
	}).Where("INSPECIONADO = 'S'").
		Count(&result)

	return result, res.Error
}
