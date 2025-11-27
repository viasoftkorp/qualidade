package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/google/uuid"
)

type InspecaoSaidaItemRepository struct {
	interfaces.IInspecaoSaidaItemRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewInspecaoSaidaItemRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (
	interfaces.IInspecaoSaidaItemRepository, error) {
	return &InspecaoSaidaItemRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *InspecaoSaidaItemRepository) BuscarInspecaoSaidaItensPeloCodigo(codigoInspecao int, filter *models.BaseFilter) ([]*models.InspecaoSaidaItem, error) {
	var result []*models.InspecaoSaidaItem

	res := repo.Uow.GetDb().
		Table(entities.InspecaoSaidaItem{}.TableName()).
		Select(queries.GetInspecaoItensSelect).
		Joins(queries.GetInspecaoItensJoin).
		Where("QA_ITEM_INSPECAO_SAIDA.CODINSP = ?", codigoInspecao).
		Order("QA_ITEM_INSPECAO_SAIDA.SEQUENCIA").
		Limit(filter.PageSize).
		Offset(filter.Skip).
		Find(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaItemRepository) BuscarQuantidadeInspecaoSaidaItensPeloCodigo(codigoInspecao int) (
	int64, error) {
	var result int64

	res := repo.Uow.GetDb().Table("QA_ITEM_INSPECAO_SAIDA").
		Where(&entities.InspecaoSaidaItem{
			CodigoInspecao: codigoInspecao,
		}).Count(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaItemRepository) BuscarInspecaoSaidaItensEntitiesPeloCodigo(codigoInspecao int) (
	[]*entities.InspecaoSaidaItem, error) {
	var result []*entities.InspecaoSaidaItem

	res := repo.Uow.GetDb().
		Where(&entities.InspecaoSaidaItem{
			CodigoInspecao: codigoInspecao,
		}).
		Find(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaItemRepository) RemoverInspecaoSaidaItensPeloCodigo(codigoInspecao int) error {
	res := repo.Uow.GetDb().
		Where(&entities.InspecaoSaidaItem{
			CodigoInspecao: codigoInspecao,
		}).
		Delete(&entities.InspecaoSaidaItem{})

	return res.Error
}

func (repo *InspecaoSaidaItemRepository) CriarItensInspecao(itensModels []*models.InspecaoSaidaItem) error {
	var itensEntities []*entities.InspecaoSaidaItem

	for _, itemModel := range itensModels {
		itemEntity := &entities.InspecaoSaidaItem{
			Id:             uuid.New(),
			Plano:          itemModel.Plano,
			Odf:            itemModel.Odf,
			Descricao:      itemModel.Descricao,
			Metodo:         itemModel.Metodo,
			Sequencia:      itemModel.Sequencia,
			Resultado:      itemModel.Resultado,
			MaiorValor:     itemModel.MaiorValor,
			MenorValor:     itemModel.MenorValor,
			CodigoInspecao: itemModel.CodigoInspecao,
			IdEmpresa:      itemModel.IdEmpresa,
			Observacao:     itemModel.Observacao,
		}

		itensEntities = append(itensEntities, itemEntity)
	}

	res := repo.Uow.GetDb().Create(&itensEntities)

	return res.Error
}

func (repo *InspecaoSaidaItemRepository) AtualizarInspecaoSaidaItens(itens []*entities.InspecaoSaidaItem) error {
	res := repo.Uow.GetDb().Save(itens)
	return res.Error
}
