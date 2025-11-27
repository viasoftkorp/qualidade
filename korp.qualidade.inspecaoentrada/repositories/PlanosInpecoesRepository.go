package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"database/sql"
)

type PlanosInspecaoRepository struct {
	interfaces.IPlanosInspecaoRepository
	Uow unit_of_work.UnitOfWork
}

func NewPlanosInspecaoRepository(uow unit_of_work.UnitOfWork) (
	interfaces.IPlanosInspecaoRepository, error) {
	return &PlanosInspecaoRepository{
		Uow: uow,
	}, nil
}

func (repo *PlanosInspecaoRepository) BuscarPlanosNovaInspecao(plano int, codigoProduto string, filter *models.BaseFilter) (
	[]models.PlanoInspecao, error) {
	var result []models.PlanoInspecao

	res := repo.Uow.GetDb().Raw(queries.GetPlanosNovaInspecao+queries.GetPlanosNovaInspecaoPagination,
		sql.Named(queries.NamedPlano, plano),
		sql.Named(queries.NamedProdutoCodigo, codigoProduto),
		sql.Named(queries.NamedSkip, filter.Skip),
		sql.Named(queries.NamedPageSize, filter.PageSize)).
		Scan(&result)

	return result, res.Error
}

func (repo *PlanosInspecaoRepository) BuscarQuantidadePlanosNovaInspecao(plano int, codigoProduto string) (
	int64, error) {
	var result int64

	res := repo.Uow.GetDb().Raw(queries.GetCountPlanosNovaInspecao,
		sql.Named(queries.NamedPlano, plano),
		sql.Named(queries.NamedProdutoCodigo, codigoProduto)).
		Count(&result)

	return result, res.Error
}

func (repo *PlanosInspecaoRepository) BuscarTodosPlanosNotaFiscalProduto(plano int, codigoProduto string) ([]models.PlanoInspecao, error) {
	var result []models.PlanoInspecao

	res := repo.Uow.GetDb().Raw(queries.GetPlanosNovaInspecao,
		sql.Named(queries.NamedPlano, plano),
		sql.Named(queries.NamedProdutoCodigo, codigoProduto)).
		Find(&result)

	return result, res.Error
}
