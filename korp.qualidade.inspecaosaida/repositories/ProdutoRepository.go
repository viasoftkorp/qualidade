package repositories

import (
	"context"
	"database/sql"
	"strings"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"gorm.io/gorm"
)

type ProdutoRepository struct {
	interfaces.IProdutoRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewProdutoRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (interfaces.IProdutoRepository, error) {
	return &ProdutoRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *ProdutoRepository) BuscarProdutoDescricao(codigoProduto string) (string, error) {
	var descricao string

	queryResult := repo.Uow.GetDb().
		Table("ESTOQUE").
		Where("CODIGO = '" + codigoProduto + "'").
		Select("DESCRI AS descricao").
		Limit(1).
		Scan(&descricao)

	if queryResult.Error != nil {
		return "", queryResult.Error
	}

	return descricao, nil
}

func (repo *ProdutoRepository) BuscarProdutoPeloCodigo(codigoProduto string) (*dto.ProdutoOutput, error) {
	var produto dto.ProdutoOutput
	var res *gorm.DB
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	query := queries.GetProdutoPeloCodigo

	res = repo.Uow.GetDb().WithContext(ctx).
		Raw(query, sql.Named(queries.NamedProdutoCodigo, codigoProduto)).
		Scan(&produto)

	if res.Error != nil {
		return nil, res.Error
	}

	return &produto, nil
}

func (repo *ProdutoRepository) BuscarProdutos(filterInput *models.BaseFilter) ([]dto.ProdutoOutput, error) {
	var produtos []dto.ProdutoOutput
	var res *gorm.DB
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	query := queries.GetProdutos
	sorting := "CODIGO ASC"
	if filterInput.Filter != "" {
		query += queries.GetProdutosFilter
	}

	query += queries.GetProdutosPaginacao
	query = strings.Replace(query, "Sorting", sorting, 1)

	res = repo.Uow.GetDb().WithContext(ctx).
		Raw(query,
			sql.Named(queries.NamedPageSize, filterInput.PageSize),
			sql.Named(queries.NamedSkip, filterInput.Skip),
			sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
			sql.Named(queries.NamedFilter, "%"+filterInput.Filter+"%")).
		Scan(&produtos)

	if res.Error != nil {
		return nil, res.Error
	}

	return produtos, nil
}

func (repo *ProdutoRepository) BuscarProdutosTotalCount(filterInput *models.BaseFilter) (int64, error) {
	var count int64
	var res *gorm.DB
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	query := queries.GetProdutosTotalCount
	if filterInput.Filter != "" {
		query += queries.GetProdutosFilter
	}

	res = repo.Uow.GetDb().WithContext(ctx).
		Raw(query,
			sql.Named(queries.NamedPageSize, filterInput.PageSize),
			sql.Named(queries.NamedSkip, filterInput.Skip),
			sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
			sql.Named(queries.NamedFilter, "%"+filterInput.Filter+"%")).
		Count(&count)

	if res.Error != nil {
		return 0, res.Error
	}

	return count, nil
}
