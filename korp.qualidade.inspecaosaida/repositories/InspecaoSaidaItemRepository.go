package repositories

import (
	"strings"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
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

	if filter.Sorting == "" {
		filter.Sorting = "QA_PLANO_INS_SAIDA.SEQUENCIA ASC"
	}

	res := repo.Uow.GetDb().
		Table(entities.InspecaoSaidaItem{}.TableName()).
		Select(queries.GetInspecaoItensJoinSelect).
		Joins(queries.GetInspecaoItensJoin).
		Where("QA_ITEM_INSPECAO_SAIDA.CODINSP = ?", codigoInspecao).
		Where(utils.ApplyAdvancedFilter(filter.AdvancedFilter)).
		Order(filter.Sorting).
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
		Table(entities.InspecaoSaidaItem{}.TableName()).
		Select(queries.GetInspecaoItensSelect).
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
			Id:                     uuid.New(),
			LegacyIdPlanoInspecao:  itemModel.LegacyIdPlanoInspecao,
			Plano:                  itemModel.Plano,
			Odf:                    itemModel.Odf,
			Descricao:              itemModel.Descricao,
			Metodo:                 itemModel.Metodo,
			Sequencia:              itemModel.Sequencia,
			Resultado:              itemModel.Resultado,
			MaiorValor:             itemModel.MaiorValor,
			MenorValor:             itemModel.MenorValor,
			CodigoInspecao:         itemModel.CodigoInspecao,
			IdEmpresa:              itemModel.IdEmpresa,
			Observacao:             itemModel.Observacao,
		}

		itensEntities = append(itensEntities, itemEntity)
	}

	res := repo.Uow.GetDb().Create(&itensEntities)

	return res.Error
}

func (repo *InspecaoSaidaItemRepository) AtualizarInspecaoSaidaItens(itens []*entities.InspecaoSaidaItem) error {
	var query = ""

	for _, item := range itens {
		query += `
			UPDATE QA_ITEM_INSPECAO_SAIDA
			SET MAIORVALOR = @MaiorValor,
			MENORVALOR = @MenorValor,
			RESULTADO = @Resultado,
			OBSERVACAO = @Observacao
			WHERE Id = @Id
		`

		query = strings.ReplaceAll(query, "@MaiorValor", item.MaiorValor.String())
		query = strings.ReplaceAll(query, "@MenorValor", item.MenorValor.String())
		query = strings.ReplaceAll(query, "@Resultado", "'"+item.Resultado+"'")
		query = strings.ReplaceAll(query, "@Observacao", "'"+item.Observacao+"'")
		query = strings.ReplaceAll(query, "@Id", "'"+item.Id.String()+"'")
	}

	res := repo.Uow.GetDb().Exec(query)
	return res.Error
}
