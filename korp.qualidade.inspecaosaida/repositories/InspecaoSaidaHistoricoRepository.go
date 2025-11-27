package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"database/sql"
	"fmt"
	"strconv"
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

	filterQuery := ""
	if filters.OrdemFabricacao != nil {
		filterQuery += "AND QA_INSPECAO_SAIDA.NUMODF = " + strconv.Itoa(*filters.OrdemFabricacao) + "\n"
	}

	if filters.CodigoProduto != nil {
		filterQuery += "AND ESTOQUE_LOCAL.CODIGO = " + *filters.CodigoProduto + "\n"
	}

	if filters.Lote != nil {
		filterQuery += "AND ESTOQUE_LOCAL.LOTE = " + *filters.Lote + "\n"
	}

	query := fmt.Sprintf(queries.GetAllInspecaoSaidaCabecalhoHistoricoInspecao, filterQuery) + queries.GetInspecaoSaidaCabecalhoHistoricoInspecaoPagination

	var args = models.QueryFilter{
		Skip:     baseFilters.Skip,
		PageSize: baseFilters.PageSize,
	}

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
		args).
		Scan(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaHistoricoRepository) CountInspecaoSaidaHistoricoCabecalho(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters) (int64, error) {
	var result int64

	filterQuery := ""
	if filters.OrdemFabricacao != nil {
		filterQuery += "AND QA_INSPECAO_SAIDA.NUMODF = " + strconv.Itoa(*filters.OrdemFabricacao) + "\n"
	}

	if filters.CodigoProduto != nil {
		filterQuery += "AND ESTOQUE_LOCAL.CODIGO = '" + *filters.CodigoProduto + "'" + "\n"
	}

	if filters.Lote != nil {
		filterQuery += "AND ESTOQUE_LOCAL.LOTE = '" + *filters.Lote + "'" + "\n"
	}

	query := fmt.Sprintf(queries.GetCountInspecaoSaidaCabecalhoHistoricoInspecao, filterQuery)

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno)).
		Count(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaHistoricoRepository) GetAllInspecaoSaidaHistoricoItems(baseFilters *models.BaseFilter, odf int) ([]dto.InspecaoSaidaHistoricoItems, error) {
	var result []dto.InspecaoSaidaHistoricoItems

	query := queries.GetAllInspecaoSaidaItensHistoricoInspecao + queries.GetInspecaoSaidaItensHistoricoInspecaoPagination

	var args = models.QueryFilter{
		Skip:     baseFilters.Skip,
		PageSize: baseFilters.PageSize,
	}

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
		sql.Named(queries.NamedOdf, odf),
		args).
		Scan(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaHistoricoRepository) CountInspecaoSaidaHistoricoItems(baseFilters *models.BaseFilter, odf int) (int64, error) {
	var result int64

	query := queries.GetCountInspecaoSaidaItensHistoricoInspecao

	res := repo.Uow.GetDb().Raw(query,
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
		sql.Named(queries.NamedOdf, odf)).
		Count(&result)

	return result, res.Error
}
