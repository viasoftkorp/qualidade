package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"gorm.io/gorm"
)

type InspecaoSaidaExecutadoWebRepository struct {
	interfaces.IInspecaoSaidaExecutadoWebRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewInspecaoSaidaExecutadoWebRepository(uow unit_of_work.UnitOfWork, baseParams *models.BaseParams) (interfaces.IInspecaoSaidaExecutadoWebRepository, error) {
	return &InspecaoSaidaExecutadoWebRepository{
		Uow:        uow,
		BaseParams: baseParams,
	}, nil
}

func (repo *InspecaoSaidaExecutadoWebRepository) BuscarInspecaoSaidaExecutadoWeb(recnoInspecao int) (*entities.InspecaoSaidaExecutadoWeb, error) {
	var result *entities.InspecaoSaidaExecutadoWeb

	res := repo.Uow.GetDb().
		Table(entities.InspecaoSaidaExecutadoWeb{}.TableName()).
		Where(&entities.InspecaoSaidaExecutadoWeb{
			RecnoInspecaoSaida: recnoInspecao,
		}).Find(&result)

	if res.Error != nil {
		if res.Error == gorm.ErrRecordNotFound {
			return nil, nil
		}
		return nil, res.Error
	}

	return result, res.Error
}

func (repo *InspecaoSaidaExecutadoWebRepository) InserirInspecaoSaidaExecutadoWeb(inspecaoSaidaExecutadaWeb *entities.InspecaoSaidaExecutadoWeb) error {
	err := repo.Uow.GetDb().Where("RECNO_INSPECAO_SAIDA = ? AND ESTORNO = ?", inspecaoSaidaExecutadaWeb.RecnoInspecaoSaida, inspecaoSaidaExecutadaWeb.Estorno).
		Delete(&entities.InspecaoSaidaExecutadoWeb{}).Error
	if err != nil {
		return err
	}

	return repo.Uow.GetDb().Create(&entities.InspecaoSaidaExecutadoWeb{
		Id:                    utils.NewGuidAsString(),
		RecnoInspecaoSaida:    inspecaoSaidaExecutadaWeb.RecnoInspecaoSaida,
		IdInspecaoSaidaSaga:   inspecaoSaidaExecutadaWeb.IdInspecaoSaidaSaga,
		Estorno:               inspecaoSaidaExecutadaWeb.Estorno,
		QuantidadeTransferida: inspecaoSaidaExecutadaWeb.QuantidadeTransferida,
		IdRnc:                 inspecaoSaidaExecutadaWeb.IdRnc,
		CodigoRnc:             inspecaoSaidaExecutadaWeb.CodigoRnc,
	}).Error
}

func (repo *InspecaoSaidaExecutadoWebRepository) RemoverSaga(idSaga string) error {
	var result *entities.InspecaoSaidaExecutadoWeb

	err := repo.Uow.GetDb().
		Table(entities.InspecaoSaidaExecutadoWeb{}.TableName()).
		Where(&entities.InspecaoSaidaExecutadoWeb{
			IdInspecaoSaidaSaga: idSaga,
		}).Find(&result).Error
	if err != nil {
		return err
	}

	if result.Estorno {
		return repo.Uow.GetDb().Where(&entities.InspecaoSaidaExecutadoWeb{IdInspecaoSaidaSaga: idSaga}).Update("ESTORNO", 0).Error
	}
	return repo.Uow.GetDb().Where(&entities.InspecaoSaidaExecutadoWeb{IdInspecaoSaidaSaga: idSaga}).Delete(&entities.InspecaoSaidaExecutadoWeb{}).Error
}
