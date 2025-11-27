package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"context"
	"database/sql"
	"fmt"
	"gorm.io/gorm"
	"strconv"
	"strings"
	"time"
)

type LocaisRepository struct {
	interfaces.ILocaisRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewLocaisRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (interfaces.ILocaisRepository, error) {
	return &LocaisRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *LocaisRepository) BuscarLocaisPrincipais(codigoProduto string) ([]dto.LocalOutput, error) {
	locais := make([]dto.LocalOutput, 0)

	if codigoProduto != "" {
		query := `SELECT CAST(LOCAIS.Id AS VARCHAR(36)) AS Id, LOCAIS.CODIGO AS Codigo, LOCAIS.DESCRICAO AS Descricao FROM LOCAIS`
		query = query + "\n" + fmt.Sprintf("INNER JOIN ESTOQUE_EMPRESA ON ESTOQUE_EMPRESA.EMPRESA_RECNO = LOCAIS.EMPRESA_RECNO AND ESTOQUE_EMPRESA.LOCAL_ENTRADA = LOCAIS.CODIGO AND ESTOQUE_EMPRESA.CODIGO = '%s'", codigoProduto)
		query = query + "\n" + "WHERE LOCAIS.EMPRESA_RECNO = ?"

		queryResult := repo.Uow.GetDb().
			Raw(query, repo.BaseParams.LegacyCompanyId).
			Scan(&locais)
		if queryResult.Error != nil {
			return nil, queryResult.Error
		}

		if len(locais) > 0 {
			return locais, nil
		}
	}

	query := `SELECT CAST(LOCAIS.Id AS VARCHAR(36)) AS Id, LOCAIS.CODIGO AS Codigo, LOCAIS.DESCRICAO AS Descricao FROM LOCAIS`
	query = query + "\n" + "WHERE LOCAL_PRINCIPAL = 'S' AND EMPRESA_RECNO = ?"
	queryResult := repo.Uow.GetDb().
		Raw(query, repo.BaseParams.LegacyCompanyId).
		Scan(&locais)
	if queryResult.Error != nil {
		return nil, queryResult.Error
	}

	return locais, nil
}

func (repo *LocaisRepository) BuscarLocaisReprovados() ([]dto.LocalOutput, error) {
	locais := make([]dto.LocalOutput, 0)

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("LOCAL_REPROVADO = 'S'").
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.LegacyCompanyId)).
		Select("CAST(LOCAIS.Id AS VARCHAR(36)) AS Id, CODIGO AS Codigo, DESCRICAO AS Descricao").
		Scan(&locais)

	if queryResult.Error != nil {
		return nil, queryResult.Error
	}

	return locais, nil
}

func (repo *LocaisRepository) BuscarLocalReprovado() (int, error) {
	var localReprovado int

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("LOCAL_REPROVADO = 'S'").
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.LegacyCompanyId)).
		Select("CODIGO AS localReprovado").
		Limit(1).
		Scan(&localReprovado)

	if queryResult.Error != nil {
		return 0, queryResult.Error
	}

	return localReprovado, nil
}

func (repo *LocaisRepository) BuscarLocalAprovado(codigoProduto string) (int, error) {
	locais, err := repo.BuscarLocaisPrincipais(codigoProduto)
	if err != nil {
		return 0, err
	}

	return strconv.Atoi(locais[0].Codigo)
}

func (repo *LocaisRepository) BuscarLocalEntrada() (int, error) {
	var localEntrada int

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("LOCAL_CQ_ENTRADA = 'S'").
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.LegacyCompanyId)).
		Select("CODIGO AS localAcabado").
		Limit(1).
		Scan(&localEntrada)

	if queryResult.Error != nil {
		return 0, queryResult.Error
	}

	return localEntrada, nil
}

func (repo *LocaisRepository) BuscarLocalDescricao(codigoLocal int) (string, error) {
	var descricao string

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("CODIGO = " + strconv.Itoa(codigoLocal)).
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.LegacyCompanyId)).
		Select("DESCRICAO AS descricao").
		Limit(1).
		Scan(&descricao)

	if queryResult.Error != nil {
		return "", queryResult.Error
	}

	return descricao, nil
}

func (repo *LocaisRepository) BuscarLocalPeloCodigo(codigoLocal int) (*dto.LocalOutput, error) {
	var local dto.LocalOutput
	var res *gorm.DB
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	query := queries.GetLocalPeloCodigo

	res = repo.Uow.GetDb().WithContext(ctx).
		Raw(query,
			sql.Named(queries.NamedLocal, strconv.Itoa(codigoLocal)),
			sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
		).
		Scan(&local)

	if res.Error != nil {
		return nil, res.Error
	}

	return &local, nil
}

func (repo *LocaisRepository) BuscarLocais(filterInput *models.BaseFilter) ([]dto.LocalOutput, error) {
	var locais []dto.LocalOutput
	var res *gorm.DB
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	query := queries.GetLocais
	sorting := "CODIGO ASC"
	if filterInput.Filter != "" {
		query += queries.GetLocaisFilter
	} else {
		query += `WHERE EMPRESA_RECNO = @` + queries.NamedEmpresaRecno + "\n"
	}

	query += queries.GetLocaisPaginacao
	query = strings.Replace(query, "Sorting", sorting, 1)

	res = repo.Uow.GetDb().WithContext(ctx).
		Raw(query,
			sql.Named(queries.NamedPageSize, filterInput.PageSize),
			sql.Named(queries.NamedSkip, filterInput.Skip),
			sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.LegacyCompanyId),
			sql.Named(queries.NamedFilter, "%"+filterInput.Filter+"%")).
		Scan(&locais)

	if res.Error != nil {
		return nil, res.Error
	}

	return locais, nil
}

func (repo *LocaisRepository) BuscarLocaisTotalCount(filterInput *models.BaseFilter) (int64, error) {
	var count int64
	var res *gorm.DB
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	query := queries.GetLocaisTotalCount
	if filterInput.Filter != "" {
		query += queries.GetLocaisFilter
	} else {
		query += `WHERE EMPRESA_RECNO = @` + queries.NamedEmpresaRecno + "\n"
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
