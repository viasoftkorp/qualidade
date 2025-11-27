package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"database/sql"
)

type PlanosInspecaoRepository struct {
	interfaces.IPlanosInspecaoRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewPlanosInspecaoRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (
	interfaces.IPlanosInspecaoRepository, error) {
	return &PlanosInspecaoRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *PlanosInspecaoRepository) BuscarPlanosNovaInspecao(recnoProcesso int, plano string, filter *models.BaseFilter) (
	[]*models.PlanoInspecao, error) {
	var result []*models.PlanoInspecao

	res := repo.Uow.GetDb().Raw(queries.GetPlanosNovaInspecao+queries.GetPlanosNovaInspecaoPagination,
		sql.Named(queries.NamedRecnoProcesso, recnoProcesso),
		sql.Named(queries.NamedPlano, plano),
		sql.Named(queries.NamedSkip, filter.Skip),
		sql.Named(queries.NamedPageSize, filter.PageSize)).
		Find(&result)

	return result, res.Error
}

func (repo *PlanosInspecaoRepository) BuscarQuantidadePlanosNovaInspecao(recnoProcesso int, plano string) (
	int64, error) {
	var result int64

	res := repo.Uow.GetDb().Raw(queries.GetCountPlanosNovaInspecao,
		sql.Named(queries.NamedRecnoProcesso, recnoProcesso),
		sql.Named(queries.NamedPlano, plano)).
		Count(&result)

	return result, res.Error
}

func (repo *PlanosInspecaoRepository) BuscarTodosPlanosOdfProduto(recnoProcesso int, plano string) ([]*models.PlanoInspecao, error) {
	var result []*models.PlanoInspecao

	res := repo.Uow.GetDb().Raw(queries.GetPlanosNovaInspecao,
		sql.Named(queries.NamedRecnoProcesso, recnoProcesso),
		sql.Named(queries.NamedPlano, plano)).
		Find(&result)

	return result, res.Error
}
