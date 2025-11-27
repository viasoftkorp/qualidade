package repositories

import (
	"context"
	"errors"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"gorm.io/gorm"
)

type InspecaoEntradaPlanoAmostragemRepository struct {
	interfaces.IInspecaoEntradaPlanoAmostragemRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewInspecaoEntradaPlanoAmostragemRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (interfaces.IInspecaoEntradaPlanoAmostragemRepository, error) {
	return &InspecaoEntradaPlanoAmostragemRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *InspecaoEntradaPlanoAmostragemRepository) GetFaixaPlanoAmostragem(quantidade float64) (*dto.PlanoAmostragemDTO, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	var entity entities.InspecaoEntradaPlanoAmostragem
	queryResult := repo.Uow.GetDb().WithContext(ctx).Table(entities.InspecaoEntradaPlanoAmostragem{}.TableName()).
		Select("QTD_MIN AS QuantidadeMinima, QTD_MAX AS QuantidadeMaxima, QTD_INSPEC AS QuantidadeInspecionar, cast(ID as varchar(36)) AS Id").
		Where("? >= QTD_MIN AND ? <= QTD_MAX", quantidade, quantidade).First(&entity)
	if queryResult.Error != nil {
		if errors.Is(queryResult.Error, gorm.ErrRecordNotFound) {
			return nil, nil
		}
		return nil, queryResult.Error
	}

	return &dto.PlanoAmostragemDTO{
		Id:                    entity.Id,
		QuantidadeMinima:      entity.QuantidadeMinima,
		QuantidadeMaxima:      entity.QuantidadeMaxima,
		QuantidadeInspecionar: entity.QuantidadeInspecionar,
	}, nil
}

func (repo *InspecaoEntradaPlanoAmostragemRepository) GetAll(filter *models.BaseFilter) (*dto.GetAllPlanoAmostragemDTO, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	entitiesArray := make([]entities.InspecaoEntradaPlanoAmostragem, 0)
	queryResult := repo.Uow.GetDb().WithContext(ctx).Table(entities.InspecaoEntradaPlanoAmostragem{}.TableName()).
		Select("QTD_MIN AS QuantidadeMinima, QTD_MAX AS QuantidadeMaxima, QTD_INSPEC AS QuantidadeInspecionar, cast(ID as varchar(36)) AS Id").
		Where(utils.ApplyAdvancedFilter(filter.AdvancedFilter)).
		Order(filter.Sorting).
		Limit(filter.PageSize).
		Offset(filter.Skip).
		Scan(&entitiesArray)
	if queryResult.Error != nil {
		return nil, queryResult.Error
	}

	var count int64
	queryResult = repo.Uow.GetDb().WithContext(ctx).Table(entities.InspecaoEntradaPlanoAmostragem{}.TableName()).Count(&count)
	if queryResult.Error != nil {
		return nil, queryResult.Error
	}

	result := &dto.GetAllPlanoAmostragemDTO{
		TotalCount: count,
		Items:      make([]dto.PlanoAmostragemDTO, 0),
	}

	for _, entity := range entitiesArray {
		result.Items = append(result.Items, dto.PlanoAmostragemDTO{
			Id:                    entity.Id,
			QuantidadeMinima:      entity.QuantidadeMinima,
			QuantidadeMaxima:      entity.QuantidadeMaxima,
			QuantidadeInspecionar: entity.QuantidadeInspecionar,
		})
	}

	return result, nil
}

func (repo *InspecaoEntradaPlanoAmostragemRepository) Create(entityToUpdate dto.PlanoAmostragemDTO) error {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	createQueryResult := repo.Uow.GetDb().WithContext(ctx).
		Create(&entities.InspecaoEntradaPlanoAmostragem{
			QuantidadeInspecionar: entityToUpdate.QuantidadeInspecionar,
			QuantidadeMaxima:      entityToUpdate.QuantidadeMaxima,
			QuantidadeMinima:      entityToUpdate.QuantidadeMinima,
		})

	return createQueryResult.Error
}

func (repo *InspecaoEntradaPlanoAmostragemRepository) Update(entityToUpdate dto.PlanoAmostragemDTO) error {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	updateQueryResult := repo.Uow.GetDb().WithContext(ctx).
		Where(&entities.InspecaoEntradaPlanoAmostragem{Id: entityToUpdate.Id}).
		Updates(&entities.InspecaoEntradaPlanoAmostragem{
			QuantidadeInspecionar: entityToUpdate.QuantidadeInspecionar,
			QuantidadeMaxima:      entityToUpdate.QuantidadeMaxima,
			QuantidadeMinima:      entityToUpdate.QuantidadeMinima,
		})

	return updateQueryResult.Error
}

func (repo *InspecaoEntradaPlanoAmostragemRepository) Delete(id string) error {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	result := repo.Uow.GetDb().WithContext(ctx).
		Where(&entities.InspecaoEntradaPlanoAmostragem{Id: id}).
		Delete(&entities.InspecaoEntradaPlanoAmostragem{})

	return result.Error
}
