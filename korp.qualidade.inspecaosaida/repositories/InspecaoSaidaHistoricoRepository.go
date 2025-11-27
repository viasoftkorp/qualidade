package repositories

import (
	"database/sql"
	"encoding/json"
	"strconv"
	"strings"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
)

type InspecaoSaidaHistoricoRepository struct {
	interfaces.IInspecaoSaidaHistoricoRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewInspecaoSaidaHistoricoRepository(uow unit_of_work.UnitOfWork, baseParams *models.BaseParams) interfaces.IInspecaoSaidaHistoricoRepository {
	return &InspecaoSaidaHistoricoRepository{
		Uow:        uow,
		BaseParams: baseParams,
	}
}

func (repo *InspecaoSaidaHistoricoRepository) GetAllInspecaoSaidaHistoricoCabecalho(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters) ([]dto.InspecaoSaidaHistoricoCabecalhoDTO, error) {
	var result []dto.InspecaoSaidaHistoricoCabecalhoDTO

	query := queries.GetAllInspecaoSaidaCabecalhoHistoricoInspecao
	query = repo.AplicarFiltros(query, baseFilters, filters)
	query += queries.GetInspecaoSaidaCabecalhoHistoricoInspecaoPagination

	if baseFilters.Sorting != "" {
		query = strings.ReplaceAll(query, "@Sorting", "ORDER BY "+baseFilters.Sorting)
	} else {
		query = strings.ReplaceAll(query, "@Sorting", "ORDER BY QA_INSPECAO_SAIDA.NUMODF")
	}

	var args = models.QueryFilter{
		Skip:     baseFilters.Skip,
		PageSize: baseFilters.PageSize,
	}

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
		args).
		Scan(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaHistoricoRepository) CountInspecaoSaidaHistoricoCabecalho(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters) (int64, error) {
	var result int64

	query := queries.GetCountInspecaoSaidaCabecalhoHistoricoInspecao
	query = repo.AplicarFiltros(query, baseFilters, filters)

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId)).
		Count(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaHistoricoRepository) GetAllInspecaoSaidaHistoricoItems(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters, odf int, codigoInspecao int) ([]dto.InspecaoSaidaHistoricoItems, error) {
	var result []dto.InspecaoSaidaHistoricoItems

	query := queries.GetAllInspecaoSaidaItensHistoricoInspecao
	query = repo.AplicarFiltros(query, baseFilters, filters)
	query += queries.GetInspecaoSaidaItensHistoricoInspecaoPagination

	if baseFilters.Sorting != "" {
		query = strings.ReplaceAll(query, "@Sorting", "ORDER BY "+baseFilters.Sorting)
	} else {
		query = strings.ReplaceAll(query, "@Sorting", "ORDER BY QA_INSPECAO_SAIDA.R_E_C_N_O_")
	}

	var args = models.QueryFilter{
		Skip:     baseFilters.Skip,
		PageSize: baseFilters.PageSize,
	}

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
		sql.Named(queries.NamedOdf, odf),
		sql.Named(queries.NamedCodigoInspecaoSaida, codigoInspecao),
		args).
		Scan(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaHistoricoRepository) CountInspecaoSaidaHistoricoItems(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters, odf int, codigoInspecao int) (int64, error) {
	var result int64

	query := queries.GetCountInspecaoSaidaItensHistoricoInspecao
	query = repo.AplicarFiltros(query, baseFilters, filters)

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
		sql.Named(queries.NamedOdf, odf),
		sql.Named(queries.NamedCodigoInspecaoSaida, codigoInspecao)).
		Count(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaHistoricoRepository) ApplyCustomAdvancedFilter(advancedFilter string) (string, string) {
	var query string
	var fieldQuery string
	var normalizedAdvancedFilter = []byte(advancedFilter)
	var deserializedAdvancedFilterToIterate models.AdvancedFilter
	json.Unmarshal(normalizedAdvancedFilter, &deserializedAdvancedFilterToIterate)

	apply := func(fieldQuery string, value string, index int) {
		if query != "" {
			query += " AND "
		}

		query += fieldQuery

		var deserializedAdvancedFilterToApply models.AdvancedFilter
		json.Unmarshal(normalizedAdvancedFilter, &deserializedAdvancedFilterToApply)

		deserializedAdvancedFilterToApply.Rules = append(deserializedAdvancedFilterToApply.Rules[:index], deserializedAdvancedFilterToApply.Rules[index+1:]...)
		normalizedAdvancedFilter, _ = json.Marshal(deserializedAdvancedFilterToApply)
	}

	for i, rule := range deserializedAdvancedFilterToIterate.Rules {
		if len(rule.Rules) == 0 {
			fieldQuery = repo.GetCustomAdvancedFilterFieldQuery(rule.Field, rule.Value)

			if fieldQuery != "" {
				apply(fieldQuery, rule.Value, i)
			}

		} else {
			for i, rule := range rule.Rules {
				fieldQuery = repo.GetCustomAdvancedFilterFieldQuery(rule.Field, rule.Value)

				if fieldQuery != "" {
					apply(fieldQuery, rule.Value, i)
				}
			}
		}
	}

	return query, string(normalizedAdvancedFilter)
}

func (repo *InspecaoSaidaHistoricoRepository) GetCustomAdvancedFilterFieldQuery(field string, value string) string {
	var normalizedValue = utils.NormalizeAdvancedFilterValue(value)

	if strings.ToUpper(field) == "ODFAPONTADA" {
		return "(PPEDLISE_ORDEM_FABRICACAO.NUMODF = '" + normalizedValue + "' OR QA_INSPECAO_SAIDA.NUMODF = '" + normalizedValue + "')"
	}
	if strings.ToUpper(field) == "REVISAO" {
		return "(PPEDLISE.REVISAO = '" + normalizedValue + "' OR FECHA.REVISAO = '" + normalizedValue + "')"
	}

	return ""
}

func (repo *InspecaoSaidaHistoricoRepository) AplicarFiltros(query string, baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters) string {
	var commonFiltersQuery = ""
	var joinFiltersQuery = ""
	var advancedFilterQuery = ""

	if filters.OrdemFabricacao != nil {
		commonFiltersQuery += "AND QA_INSPECAO_SAIDA.NUMODF = " + strconv.Itoa(*filters.OrdemFabricacao) + "\n"
	}

	if filters.CodigoProduto != nil {
		commonFiltersQuery += "AND ESTOQUE_LOCAL.CODIGO = '" + *filters.CodigoProduto + "'" + "\n"
	}

	if filters.Lote != nil {
		commonFiltersQuery += "AND ESTOQUE_LOCAL.LOTE = '" + *filters.Lote + "'" + "\n"
	}

	if len(filters.ObservacoesMetricas) > 0 {
		var observacoesMetricasLikeQuery = ""

		for index, observacao := range filters.ObservacoesMetricas {
			if index == 0 {
				observacoesMetricasLikeQuery += "("
			}

			observacoesMetricasLikeQuery += "QA_ITEM_INSPECAO_SAIDA.OBSERVACAO LIKE '%" + observacao + "%'"

			if index == len(filters.ObservacoesMetricas)-1 {
				observacoesMetricasLikeQuery += ")"
			} else {
				observacoesMetricasLikeQuery += " OR "
			}
		}

		joinFiltersQuery +=
			"JOIN QA_ITEM_INSPECAO_SAIDA ON QA_ITEM_INSPECAO_SAIDA.CODINSP = QA_INSPECAO_SAIDA.CODINSP\n" +
				"AND " + observacoesMetricasLikeQuery + "\n"
	}

	if baseFilters.AdvancedFilter != "" {
		customAdvancedFilterQuery, normalizedAdvancedFilter := repo.ApplyCustomAdvancedFilter(baseFilters.AdvancedFilter)

		if customAdvancedFilterQuery != "" {
			var normalAdvancedFilterQuery = utils.ApplyAdvancedFilter(normalizedAdvancedFilter)

			if normalAdvancedFilterQuery != "" {
				advancedFilterQuery = "AND " + normalAdvancedFilterQuery + " AND " + customAdvancedFilterQuery
			} else {
				advancedFilterQuery = "AND " + customAdvancedFilterQuery
			}
		} else {
			advancedFilterQuery = "AND " + utils.ApplyAdvancedFilter(baseFilters.AdvancedFilter)
		}
	}

	query = strings.ReplaceAll(query, "@JoinFilters", joinFiltersQuery)
	query = strings.ReplaceAll(query, "@CommonFilters", commonFiltersQuery)
	query = strings.ReplaceAll(query, "@AdvancedFilter", advancedFilterQuery)

	return query
}
