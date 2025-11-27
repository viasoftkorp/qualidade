package repositories

import (
	"context"
	"database/sql"
	"encoding/json"
	"errors"
	"strconv"
	"strings"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/queries"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
	"gorm.io/gorm"
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

func (repo *NotaFiscalRepository) BuscarNotaFiscal(recnoItemNotaFiscal int, notaFiscal int, lote string) (models.NotaFiscalModel, error) {
	var result models.NotaFiscalModel

	query := queries.GetNotaFiscal

	if recnoItemNotaFiscal > 0 {
		query += ` WHERE HISTLISE.R_E_C_N_O_ = @` + queries.NamedRecnoItemNotaFiscal + `
			AND @` + queries.NamedLote + ` = ISNULL(COALESCE(HISTLISE_RATEIO_LOTE.LOTE, HISTLISE.LOTE), 0)`
	} else {
		query += ` WHERE HISTLISE.EMPRESA_RECNO = @` + queries.NamedEmpresaRecno + `
			AND HISTLISE.NFISCAL = @` + queries.NamedNotaFiscal + `
			AND @` + queries.NamedLote + ` = ISNULL(COALESCE(HISTLISE_RATEIO_LOTE.LOTE, HISTLISE.LOTE), 0)`
	}

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Raw(query,
		sql.Named(queries.NamedRecnoItemNotaFiscal, recnoItemNotaFiscal),
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
		sql.Named(queries.NamedNotaFiscal, notaFiscal),
		sql.Named(queries.NamedLote, lote)).
		Scan(&result)

	return result, res.Error
}

func (repo *NotaFiscalRepository) BuscarNotasFiscais(baseFilters *models.BaseFilter, filters *dto.NotaFiscalFilters) ([]models.NotaFiscalModel, error) {
	var result []models.NotaFiscalModel

	query := queries.GetNotasFiscaisQuery
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

func (repo *NotaFiscalRepository) BuscarNotasFiscaisTotalCount(baseFilters *models.BaseFilter, filters *dto.NotaFiscalFilters) (int64, error) {
	var result int64

	query := queries.GetNotasFiscaisCount
	query = repo.AplicarFiltros(query, baseFilters, filters)

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId)).Count(&result)

	return result, res.Error
}

func (repo *NotaFiscalRepository) AplicarFiltros(query string, baseFilters *models.BaseFilter, filters *dto.NotaFiscalFilters) string {
	var commonFiltersQuery = ""
	var joinFiltersQuery = ""
	var advancedFilterQuery = ""

	if filters.NotaFiscal != nil {
		commonFiltersQuery += "AND HISTLISE.NFISCAL = " + strconv.Itoa(*filters.NotaFiscal) + "\n"
	}
	if filters.Lote != nil {
		commonFiltersQuery += "AND '" + *filters.Lote + "' LIKE ISNULL(COALESCE(HISTLISE_RATEIO_LOTE.LOTE, HISTLISE.LOTE), 0)" + "\n"
	}
	if filters.CodigoProduto != nil {
		commonFiltersQuery += "AND HISTLISE.ITEM LIKE '" + *filters.CodigoProduto + "'\n"
	}
	if filters.Fornecedor != nil {
		commonFiltersQuery += "AND FORNECED.RASSOC LIKE '" + *filters.Fornecedor + "'\n"
	}
	if filters.DataEntrada != nil {
		dataEntrega := *filters.DataEntrada
		commonFiltersQuery += "AND CAST(HISTLISE.DTENT AS DATE) = '" + dataEntrega.Format("2006-01-02") + "'\n"
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
			"JOIN QA_INSPECAO_ENTRADA ON QA_INSPECAO_ENTRADA.CODNOTA = HISTLISE.NFISCAL\n" +
				"AND QA_INSPECAO_ENTRADA.LOTE = ISNULL(COALESCE(HISTLISE_RATEIO_LOTE.LOTE, HISTLISE.LOTE), 0)\n" +
				"AND QA_INSPECAO_ENTRADA.R_E_C_N_O_ NOT IN (SELECT DISTINCT RECNO_INSPECAO_ENTRADA FROM InspecaoEntradaExecutadoWeb)\n" +
				"JOIN QA_ITEM_INSPECAO_ENTRADA ON QA_ITEM_INSPECAO_ENTRADA.CODINSPECAO = QA_INSPECAO_ENTRADA.COD_INSP\n" +
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

func (repo *NotaFiscalRepository) UpdateNotaFiscalDadosAdicionais(idNotaFiscal string, input *dto.NotaFiscalDadosAdicionaisDTO) error {
	var dadosAdicionais *entities.NotaFiscalDadosAdicionais

	res := repo.Uow.GetDb().
		Table(entities.NotaFiscalDadosAdicionais{}.TableName()).
		Where("IdNotaFiscal = ?", idNotaFiscal).First(&dadosAdicionais)

	if res.Error != nil {
		if errors.Is(res.Error, gorm.ErrRecordNotFound) {
			dadosAdicionais = nil
		} else {
			return res.Error
		}
	}

	if dadosAdicionais == nil {
		var dadosAdicionaisToCreate = entities.NotaFiscalDadosAdicionais{
			Id:           uuid.New().String(),
			IdNotaFiscal: idNotaFiscal,
			Observacao:   input.Observacao,
		}

		res = repo.Uow.GetDb().
			Table(entities.NotaFiscalDadosAdicionais{}.TableName()).
			Create(&dadosAdicionaisToCreate)
	} else {
		res = repo.Uow.GetDb().
			Table(entities.NotaFiscalDadosAdicionais{}.TableName()).
			Where("IdNotaFiscal = ?", idNotaFiscal).
			Update("OBSERVACAO", input.Observacao)
	}

	return res.Error
}

func (repo *NotaFiscalRepository) ApplyCustomAdvancedFilter(advancedFilter string) (string, string) {
	var query string
	var normalizedAdvancedFilter = []byte(advancedFilter)
	var deserializedAdvancedFilterToIterate models.AdvancedFilter
	json.Unmarshal(normalizedAdvancedFilter, &deserializedAdvancedFilterToIterate)

	apply := func(field string, value string) {
		fieldQuery := repo.GetCustomAdvancedFilterFieldQuery(field, value)

		if fieldQuery == "" {
			return
		}

		if query != "" {
			query += " AND "
		}

		query += fieldQuery

		var deserializedAdvancedFilterToApply models.AdvancedFilter
		json.Unmarshal(normalizedAdvancedFilter, &deserializedAdvancedFilterToApply)

		for index, rule := range deserializedAdvancedFilterToApply.Rules {
			if strings.EqualFold(rule.Field, field) {
				deserializedAdvancedFilterToApply.Rules = append(deserializedAdvancedFilterToApply.Rules[:index], deserializedAdvancedFilterToApply.Rules[index+1:]...)
				break
			}
		}

		normalizedAdvancedFilter, _ = json.Marshal(deserializedAdvancedFilterToApply)
	}

	for _, rule := range deserializedAdvancedFilterToIterate.Rules {
		if len(rule.Rules) == 0 {
			apply(rule.Field, rule.Value)
		} else {
			for _, rule := range rule.Rules {
				apply(rule.Field, rule.Value)
			}
		}
	}

	return query, string(normalizedAdvancedFilter)
}

func (repo *NotaFiscalRepository) GetCustomAdvancedFilterFieldQuery(field string, value string) string {
	var normalizedValue = utils.NormalizeAdvancedFilterValue(value)

	if strings.ToUpper(field) == "LOTE" {
		return "('" + normalizedValue + "' = ISNULL(COALESCE(HISTLISE_RATEIO_LOTE.LOTE, HISTLISE.LOTE), 0))"
	}
	if strings.ToUpper(field) == "QUANTIDADE" {
		return "('" + normalizedValue + "' = COALESCE(HISTLISE_RATEIO_LOTE.QTD, HISTLISE.QTENT))"
	}

	return ""
}

func (repo *NotaFiscalRepository) BuscarCaracteristicaItemNotaFiscal(recnoItemNotaFiscal int, quantidade decimal.Decimal) (models.CaracteristicaItemNotaFiscalModel, error) {
	var result models.CaracteristicaItemNotaFiscalModel

	query := queries.GetCaracteristicaItemNotaFiscal

	quantidadeFloat := utils.DecimalToFloat64(quantidade)

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Raw(query,
		sql.Named(queries.NamedRecnoItemNotaFiscal, recnoItemNotaFiscal),
		sql.Named(queries.NamedQuantidade, quantidadeFloat)).
		Scan(&result)

	return result, res.Error
}
