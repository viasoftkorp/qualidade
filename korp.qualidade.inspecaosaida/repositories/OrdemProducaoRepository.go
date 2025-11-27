package repositories

import (
	"database/sql"
	"encoding/json"
	"errors"
	"strconv"
	"strings"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"gorm.io/gorm"
)

type OrdemProducaoRepository struct {
	interfaces.IOrdemProducaoRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewOrdemProducaoRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (
	interfaces.IOrdemProducaoRepository, error) {
	return &OrdemProducaoRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *OrdemProducaoRepository) BuscarOrdensInspecao(baseFilters *models.BaseFilter, filters *dto.OrdemProducaoFilters) ([]models.OrdemProducao, error) {
	var result []models.OrdemProducao

	query := queries.GetOrdensInspecaoQuery
	query = repo.AplicarFiltros(query, baseFilters, filters)
	query += queries.GetOrdensInspecaoPagination

	if baseFilters.Sorting != "" {
		query = strings.ReplaceAll(query, "@Sorting", "ORDER BY "+baseFilters.Sorting)
	} else {
		query = strings.ReplaceAll(query, "@Sorting", "ORDER BY Plano, Odf")
	}

	var args = models.QueryFilter{
		Skip:     baseFilters.Skip,
		PageSize: baseFilters.PageSize,
	}

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
		sql.Named(queries.NamedFilter, "%"+baseFilters.Filter+"%"), args).
		Scan(&result)

	return result, res.Error
}

func (repo *OrdemProducaoRepository) BuscarQuantidadeOrdensInspecao(baseFilters *models.BaseFilter, filters *dto.OrdemProducaoFilters) (int64, error) {
	var result int64

	query := queries.GetQuantidadeOrdensInspecao + "\n"
	query = repo.AplicarFiltros(query, baseFilters, filters)

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId)).
		Count(&result)

	return result, res.Error
}

func (repo *OrdemProducaoRepository) AplicarFiltros(query string, baseFilters *models.BaseFilter, filters *dto.OrdemProducaoFilters) string {
	var commonFiltersQuery = ""
	var joinFiltersQuery = ""
	var advancedFilterQuery = ""

	if filters.Odf != nil {
		commonFiltersQuery += "AND PPEDLISE.NUMODF = " + strconv.Itoa(*filters.Odf) + "\n"
	}
	if filters.Lote != nil {
		commonFiltersQuery += "AND PPEDLISE.LOTE LIKE '" + *filters.Lote + "'\n"
	}
	if filters.CodigoProduto != nil {
		commonFiltersQuery += "AND PPEDLISE.CODPCA LIKE '" + *filters.CodigoProduto + "'\n"
	}
	if filters.DataInicio != nil {
		dataInicio := *filters.DataInicio
		commonFiltersQuery += "AND CAST(PPEDLISE.DTINICIO AS DATE) = '" + dataInicio.Format("2006-01-02") + "'\n"
	}
	if filters.DataEntrega != nil {
		dataEntrega := *filters.DataEntrega
		commonFiltersQuery += "AND CAST(PPEDLISE.DTENPD AS DATE) = '" + dataEntrega.Format("2006-01-02") + "'\n"
	}
	if filters.DataEmissao != nil {
		dataEmissao := *filters.DataEmissao
		commonFiltersQuery += "AND CAST(PPEDLISE.DTEMISSAO AS DATE) = '" + dataEmissao.Format("2006-01-02") + "'\n"
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
			"JOIN QA_INSPECAO_SAIDA ON QA_INSPECAO_SAIDA.NUMODF = COALESCE(HISREAL.ODF, ESTOQUE_LOCAL.ODF)\n" +
			"AND QA_INSPECAO_SAIDA.EMPRESA_RECNO = ESTOQUE_LOCAL.EMPRESA_RECNO\n" +
			"AND QA_INSPECAO_SAIDA.LOTE = ESTOQUE_LOCAL.LOTE\n" +
			"AND QA_INSPECAO_SAIDA.R_E_C_N_O_ NOT IN (SELECT DISTINCT RECNO_INSPECAO_SAIDA FROM InspecaoSaidaExecutadoWeb)\n" +
			"AND QA_INSPECAO_SAIDA.INSPECIONADO != 'S'\n" +
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

func (repo *OrdemProducaoRepository) BuscarOrdem(odf int) *models.OrdemProducao {
	var result models.OrdemProducao

	query := queries.GetOrdensInspecaoQuery
	query = query + queries.GetOrdensInspecaoOdfFilter

	query = strings.ReplaceAll(query, "@JoinFilters", "")
	query = strings.ReplaceAll(query, "@CommonFilters", "")
	query = strings.ReplaceAll(query, "@AdvancedFilter", "")

	res := repo.Uow.GetDb().Raw(query,
		odf,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
	).
		First(&result)

	if res.Error != nil {
		return nil
	}

	return &result
}

func (repo *OrdemProducaoRepository) BuscarEstoqueLocalRelatorio(odf int) *models.OrdemProducao {
	var result models.OrdemProducao

	query := queries.GetEstoqueLocalRelatorio

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
		sql.Named(queries.NamedOdf, odf),
	).
		First(&result)

	if res.Error != nil {
		return nil
	}

	return &result
}

func (repo *OrdemProducaoRepository) BuscarOrdemPaiHistoricoMovimentacao(lote, codigoProduto string, localDestino int) (*int, error) {
	var Odf int
	res := repo.Uow.GetDb().Raw(`SELECT TOP 1 ODF as Odf FROM HISREAL WHERE HISREAL.LOTE = ? AND HISREAL.CODIGO = ? AND HISREAL.LOCAL_DESTINO = ? AND
		HISREAL.FORMA = 'E' AND HISREAL.ESTORNADO_APT_PRODUCAO = 'N' AND HISREAL.EMPRESA_RECNO = ?`, lote, codigoProduto, localDestino, repo.BaseParams.LegacyCompanyId).
		First(&Odf)

	if res.Error != nil {
		if errors.Is(res.Error, gorm.ErrRecordNotFound) {
			return nil, nil
		}
		return nil, res.Error
	}

	return &Odf, nil
}

func (repo *OrdemProducaoRepository) ApplyCustomAdvancedFilter(advancedFilter string) (string, string) {
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

func (repo *OrdemProducaoRepository) GetCustomAdvancedFilterFieldQuery(field string, value string) string {
	var normalizedValue = utils.NormalizeAdvancedFilterValue(value)

	if strings.ToUpper(field) == "CLIENTE" {
		return "(CRM_PEDIDO.CLIENTE = '" + normalizedValue + "' OR PPEDLISE.CLIENTE = '" + normalizedValue + "' OR FECHA.CODCLI = '" + normalizedValue + "')"
	}
	if strings.ToUpper(field) == "ODFAPONTADA" {
		return "(PPEDLISE_ORDEM_FABRICACAO.NUMODF = '" + normalizedValue + "' OR ESTOQUE_LOCAL.ODF = '" + normalizedValue + "')"
	}
	if strings.ToUpper(field) == "NUMEROPEDIDO" {
		return "(PPEDLISE.NUMPED = '" + normalizedValue + "' OR FECHA.NUMPED = '" + normalizedValue + "')"
	}
	if strings.ToUpper(field) == "SITUACAO" {
		return "(PPEDLISE.SITUACAO = '" + normalizedValue + "' OR FECHA.SITUACAO = '" + normalizedValue + "')"
	}
	if strings.ToUpper(field) == "REVISAO" {
		return "(PPEDLISE.REVISAO = '" + normalizedValue + "' OR FECHA.REVISAO = '" + normalizedValue + "')"
	}
	if strings.ToUpper(field) == "DATAINICIO" {
		return "(PPEDLISE.DTINICIO = '" + normalizedValue + "' OR FECHA.DTINIC = '" + normalizedValue + "')"
	}
	if strings.ToUpper(field) == "DATAENTREGA" {
		return "(PPEDLISE.DTENPD = '" + normalizedValue + "' OR FECHA.DTENTR = '" + normalizedValue + "')"
	}
	if strings.ToUpper(field) == "DATAEMISSAO" {
		return "(PPEDLISE.DTEMISSAO = '" + normalizedValue + "' OR FECHA.DTEMIS = '" + normalizedValue + "')"
	}
	if strings.ToUpper(field) == "QUANTIDADEORDEM" {
		return "(PPEDLISE.QTPEDI = " + normalizedValue + " OR FECHA.QTPEDE = " + normalizedValue + " OR ESTOQUE_PEDIDO_VENDA_ESTOQUE_LOCAL.QUANTIDADE = " + normalizedValue + ")"
	}

	return ""
}
