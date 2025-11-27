package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"database/sql"
	"errors"
	"gorm.io/gorm"
	"strconv"
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
	query = repo.AplicarFiltros(query, filters)
	query += queries.GetOrdensInspecaoPagination

	var args = models.QueryFilter{
		Skip:     baseFilters.Skip,
		PageSize: baseFilters.PageSize,
	}

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
		sql.Named(queries.NamedFilter, "%"+baseFilters.Filter+"%"), args).
		Scan(&result)

	return result, res.Error
}

func (repo *OrdemProducaoRepository) BuscarQuantidadeOrdensInspecao(baseFilters *models.BaseFilter, filters *dto.OrdemProducaoFilters) (int64, error) {
	var result int64

	query := queries.GetQuantidadeOrdensInspecao + "\n"
	query = repo.AplicarFiltros(query, filters)

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno)).
		Count(&result)

	return result, res.Error
}

func (repo *OrdemProducaoRepository) AplicarFiltros(query string, filters *dto.OrdemProducaoFilters) string {
	if filters.Odf != nil {
		query += "AND PPEDLISE.NUMODF = " + strconv.Itoa(*filters.Odf) + "\n"
	}
	if filters.Lote != nil {
		query += "AND PPEDLISE.LOTE LIKE '" + *filters.Lote + "'\n"
	}
	if filters.CodigoProduto != nil {
		query += "AND PPEDLISE.CODPCA LIKE '" + *filters.CodigoProduto + "'\n"
	}
	if filters.DataInicio != nil {
		dataInicio := *filters.DataInicio
		query += "AND CAST(PPEDLISE.DTINICIO AS DATE) = '" + dataInicio.Format("2006-01-02") + "'\n"
	}
	if filters.DataEntrega != nil {
		dataEntrega := *filters.DataEntrega
		query += "AND CAST(PPEDLISE.DTENPD AS DATE) = '" + dataEntrega.Format("2006-01-02") + "'\n"
	}
	if filters.DataEmissao != nil {
		dataEmissao := *filters.DataEmissao
		query += "AND CAST(PPEDLISE.DTEMISSAO AS DATE) = '" + dataEmissao.Format("2006-01-02") + "'\n"
	}

	return query
}

func (repo *OrdemProducaoRepository) BuscarOrdem(odf int) *models.OrdemProducao {
	var result models.OrdemProducao

	query := queries.GetOrdensInspecaoQuery
	query = query + queries.GetOrdensInspecaoOdfFilter

	res := repo.Uow.GetDb().Raw(query,
		odf,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
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
		HISREAL.FORMA = 'E' AND HISREAL.ESTORNADO_APT_PRODUCAO = 'N' AND HISREAL.EMPRESA_RECNO = ?`, lote, codigoProduto, localDestino, repo.BaseParams.CompanyRecno).
		First(&Odf)

	if res.Error != nil {
		if errors.Is(res.Error, gorm.ErrRecordNotFound) {
			return nil, nil
		}
		return nil, res.Error
	}

	return &Odf, nil
}
