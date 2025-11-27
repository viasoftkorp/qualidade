package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"database/sql"
	"github.com/google/uuid"
)

type InspecaoEntradaItemRepository struct {
	interfaces.IInspecaoEntradaItemRepository
	Uow unit_of_work.UnitOfWork
}

func NewInspecaoEntradaItemRepository(uow unit_of_work.UnitOfWork) (
	interfaces.IInspecaoEntradaItemRepository, error) {
	return &InspecaoEntradaItemRepository{
		Uow: uow,
	}, nil
}

func (repo *InspecaoEntradaItemRepository) BuscarInspecaoEntradaItensPeloCodigo(codigoProduto string, codigoInspecao int, filter *models.BaseFilter) ([]models.InspecaoEntradaItem, error) {
	var result []models.InspecaoEntradaItem

	res := repo.Uow.GetDb().
		Table(entities.InspecaoEntradaItem{}.TableName()).
		Select(queries.GetInspecaoItensSelect).
		Joins(queries.GetInspecaoItensJoin, sql.Named(queries.NamedProdutoCodigo, codigoProduto)).
		Where("QA_ITEM_INSPECAO_ENTRADA.CODINSPECAO = ?", codigoInspecao).
		Order("QA_ITEM_INSPECAO_ENTRADA.SEQUENCIA").
		Limit(filter.PageSize).
		Offset(filter.Skip).
		Find(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaItemRepository) BuscarQuantidadeInspecaoEntradaItensPeloCodigo(codigoInspecao int) (
	int64, error) {
	var result int64

	res := repo.Uow.GetDb().Table("QA_ITEM_INSPECAO_ENTRADA").Where(&entities.InspecaoEntradaItem{CodigoInspecao: codigoInspecao}).Count(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaItemRepository) BuscarInspecaoEntradaItensEntitiesPeloCodigo(codigoInspecao int) (
	[]*entities.InspecaoEntradaItem, error) {
	var result []*entities.InspecaoEntradaItem

	res := repo.Uow.GetDb().
		Where(&entities.InspecaoEntradaItem{
			CodigoInspecao: codigoInspecao,
		}).
		Find(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaItemRepository) RemoverInspecaoEntradaItensPeloCodigo(codigoInspecao int) error {
	res := repo.Uow.GetDb().
		Where(&entities.InspecaoEntradaItem{
			CodigoInspecao: codigoInspecao,
		}).
		Delete(&entities.InspecaoEntradaItem{})

	return res.Error
}

func (repo *InspecaoEntradaItemRepository) CriarItensInspecao(itensModels []models.InspecaoEntradaItem) error {
	var itensEntities []*entities.InspecaoEntradaItem

	for _, itemModel := range itensModels {
		itemEntity := &entities.InspecaoEntradaItem{
			Id:                     uuid.New(),
			Plano:                  itemModel.Plano,
			Descricao:              itemModel.Descricao,
			Metodo:                 itemModel.Metodo,
			Sequencia:              itemModel.Sequencia,
			Resultado:              itemModel.Resultado,
			MaiorValorInspecionado: itemModel.MaiorValorInspecionado,
			MenorValorInspecionado: itemModel.MenorValorInspecionado,
			CodigoInspecao:         itemModel.CodigoInspecao,
			Observacao:             itemModel.Observacao,
		}

		itensEntities = append(itensEntities, itemEntity)
	}

	res := repo.Uow.GetDb().Create(&itensEntities)

	return res.Error
}

func (repo *InspecaoEntradaItemRepository) AtualizarInspecaoEntradaItens(itens []*entities.InspecaoEntradaItem) error {
	res := repo.Uow.GetDb().Save(itens)
	return res.Error
}
