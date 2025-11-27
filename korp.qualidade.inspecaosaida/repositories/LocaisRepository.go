package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
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

func (repo *LocaisRepository) BuscarLocalRetrabalho() (int, error) {
	var localRetrabalho int

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("BLOQUEAR_MOVIMENTACAO = 'S'").
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.CompanyRecno)).
		Select("CODIGO AS localRetrabalho").
		Limit(1).
		Scan(&localRetrabalho)

	if queryResult.Error != nil {
		return 0, queryResult.Error
	}

	return localRetrabalho, nil
}

func (repo *LocaisRepository) BuscarLocalReprovado() (int, error) {
	var localReprovado int

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("LOCAL_REPROVADO = 'S'").
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.CompanyRecno)).
		Select("CODIGO AS localReprovado").
		Limit(1).
		Scan(&localReprovado)

	if queryResult.Error != nil {
		return 0, queryResult.Error
	}

	return localReprovado, nil
}

func (repo *LocaisRepository) BuscarLocalAprovado() (int, error) {
	var localAcabado int

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("LOCAL_ACABADO = 'S'").
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.CompanyRecno)).
		Select("CODIGO AS localAcabado").
		Limit(1).
		Scan(&localAcabado)

	if queryResult.Error != nil {
		return 0, queryResult.Error
	}

	return localAcabado, nil
}

func (repo *LocaisRepository) BuscarLocalSaida() (int, error) {
	var localSaida int

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("LOCAL_CQ_SAIDA = 'S'").
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.CompanyRecno)).
		Select("CODIGO AS localAcabado").
		Limit(1).
		Scan(&localSaida)

	if queryResult.Error != nil {
		return 0, queryResult.Error
	}

	return localSaida, nil
}

func (repo *LocaisRepository) BuscarLocalDescricao(codigoLocal int) (string, error) {
	var descricao string

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("CODIGO = " + strconv.Itoa(codigoLocal)).
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.CompanyRecno)).
		Select("DESCRICAO AS descricao").
		Limit(1).
		Scan(&descricao)

	if queryResult.Error != nil {
		return "", queryResult.Error
	}

	return descricao, nil
}

func (repo *LocaisRepository) BuscarLocaisAprovados() ([]map[string]interface{}, error) {
	locais := make([]map[string]interface{}, 0)

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("LOCAL_ACABADO = 'S'").
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.CompanyRecno)).
		Select("CODIGO AS codigo, DESCRICAO AS descricao").
		Scan(&locais)

	if queryResult.Error != nil {
		return nil, queryResult.Error
	}

	return locais, nil
}

func (repo *LocaisRepository) BuscarLocaisReprovados() ([]map[string]interface{}, error) {
	locais := make([]map[string]interface{}, 0)

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("LOCAL_REPROVADO = 'S'").
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.CompanyRecno)).
		Select("CODIGO AS codigo, DESCRICAO AS descricao").
		Scan(&locais)

	if queryResult.Error != nil {
		return nil, queryResult.Error
	}

	return locais, nil
}

func (repo *LocaisRepository) BuscarLocaisRetrabalho() ([]map[string]interface{}, error) {
	locais := make([]map[string]interface{}, 0)

	queryResult := repo.Uow.GetDb().
		Table("LOCAIS").
		Where("BLOQUEAR_MOVIMENTACAO = 'S'").
		Where(fmt.Sprintf("EMPRESA_RECNO = %d", repo.BaseParams.CompanyRecno)).
		Select("CODIGO AS codigo, DESCRICAO AS descricao").
		Scan(&locais)

	if queryResult.Error != nil {
		return nil, queryResult.Error
	}

	return locais, nil
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
			sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
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
			sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
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
			sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
			sql.Named(queries.NamedFilter, "%"+filterInput.Filter+"%")).
		Count(&count)

	if res.Error != nil {
		return 0, res.Error
	}

	return count, nil
}
