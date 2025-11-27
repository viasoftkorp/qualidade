package repositories

import (
	"context"
	"database/sql"
	"strconv"
	"strings"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/queries"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
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

func (repo *InspecaoEntradaHistoricoRepository) BuscarNotasFiscaisHistorico(baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) ([]models.NotaFiscalModel, error) {
	var result []models.NotaFiscalModel

	query := queries.GetNotasFiscaisHistoricoQuery
	query = repo.AplicarFiltros(query, baseFilters, filters)
	query += queries.GetNotasFiscaisPagination

	if baseFilters.Sorting != "" {
		query = strings.ReplaceAll(query, "@Sorting", "ORDER BY "+baseFilters.Sorting)
	} else {
		query = strings.ReplaceAll(query, "@Sorting", "ORDER BY HISTLISE.R_E_C_N_O_")
	}

	var args = models.QueryFilter{
		Skip:     baseFilters.Skip,
		PageSize: baseFilters.PageSize,
	}
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId), args).
		Scan(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaHistoricoRepository) BuscarNotasFiscaisHistoricoTotalCount(baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) (int64, error) {
	var result int64

	query := queries.GetNotasFiscaisHistoricoCount
	query = repo.AplicarFiltros(query, baseFilters, filters)

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId)).Count(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaHistoricoRepository) BuscarInspecoesEntradaHistorico(recnoItemNotaFiscal int, notaFiscal int, lote string, baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) ([]dto.InspecaoEntradaHistoricoItems, error) {
	var result []dto.InspecaoEntradaHistoricoItems

	query := queries.GetAllInspecaoEntradaItensHistoricoInspecao
	query = repo.AplicarFiltros(query, baseFilters, filters)
	query += queries.GetInspecaoEntradaItensHistoricoInspecaoPagination

	if baseFilters.Sorting != "" {
		query = strings.ReplaceAll(query, "@Sorting", "ORDER BY "+baseFilters.Sorting)
	} else {
		query = strings.ReplaceAll(query, "@Sorting", "ORDER BY QA_INSPECAO_ENTRADA.R_E_C_N_O_")
	}

	var args = models.QueryFilter{
		Skip:     baseFilters.Skip,
		PageSize: baseFilters.PageSize,
	}

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
		sql.Named(queries.NamedNotaFiscal, notaFiscal),
		sql.Named(queries.NamedLote, lote),
		sql.Named(queries.NamedRecnoItemNotaFiscal, recnoItemNotaFiscal),
		args).
		Scan(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaHistoricoRepository) BuscarQuantidadeInspecoesEntradaHistorico(recnoItemNotaFiscal int, notaFiscal int, lote string, baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) (int64, error) {
	var result int64

	query := queries.GetInspecoesEntradaItensHistoricoCount
	query = repo.AplicarFiltros(query, baseFilters, filters)

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Raw(query,
		sql.Named(queries.NamedLote, lote), sql.Named(queries.NamedNotaFiscal, notaFiscal),
		sql.Named(queries.NamedRecnoItemNotaFiscal, recnoItemNotaFiscal)).
		Count(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaHistoricoRepository) AplicarFiltros(query string, baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) string {
	var commonFiltersQuery = ""
	var joinFiltersQuery = ""
	var advancedFilterQuery = ""

	if filters.NotaFiscal != nil {
		commonFiltersQuery += "AND QA_INSPECAO_ENTRADA.CODNOTA = " + strconv.Itoa(*filters.NotaFiscal) + "\n"
	}

	if filters.CodigoProduto != nil {
		commonFiltersQuery += "AND HISTLISE.ITEM = " + *filters.CodigoProduto + "\n"
	}

	if filters.Lote != nil {
		commonFiltersQuery += "AND QA_INSPECAO_ENTRADA.LOTE = '" + *filters.Lote + "'" + "\n"
	}

	if len(filters.ObservacoesMetricas) > 0 {
		var observacoesMetricasLikeQuery = ""

		for index, observacao := range filters.ObservacoesMetricas {
			if index == 0 {
				observacoesMetricasLikeQuery += "("
			}

			observacoesMetricasLikeQuery += "QA_ITEM_INSPECAO_ENTRADA.OBSERVACAO LIKE '%" + observacao + "%'"

			if index == len(filters.ObservacoesMetricas)-1 {
				observacoesMetricasLikeQuery += ")"
			} else {
				observacoesMetricasLikeQuery += " OR "
			}
		}

		joinFiltersQuery +=
			"JOIN QA_ITEM_INSPECAO_ENTRADA ON QA_ITEM_INSPECAO_ENTRADA.CODINSPECAO = QA_INSPECAO_ENTRADA.COD_INSP\n" +
				"AND " + observacoesMetricasLikeQuery + "\n"
	}

	if baseFilters.AdvancedFilter != "" {
		advancedFilterQuery = "AND " + utils.ApplyAdvancedFilter(baseFilters.AdvancedFilter)
	}

	query = strings.ReplaceAll(query, "@JoinFilters", joinFiltersQuery)
	query = strings.ReplaceAll(query, "@CommonFilters", commonFiltersQuery)
	query = strings.ReplaceAll(query, "@AdvancedFilter", advancedFilterQuery)

	return query
}
