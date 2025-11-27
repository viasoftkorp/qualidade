package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
)

type InspecaoEntradaExecutadoWebRepository struct {
	interfaces.IInspecaoEntradaExecutadoWebRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewInspecaoEntradaExecutadoWebRepository(uow unit_of_work.UnitOfWork, baseParams *models.BaseParams) (interfaces.IInspecaoEntradaExecutadoWebRepository, error) {
	return &InspecaoEntradaExecutadoWebRepository{
		Uow:        uow,
		BaseParams: baseParams,
	}, nil
}

func (repo *InspecaoEntradaExecutadoWebRepository) BuscarInspecaoEntradaExecutadoWeb(recnoInspecao int) (*entities.InspecaoEntradaExecutadoWeb, error) {
	var result *entities.InspecaoEntradaExecutadoWeb

	res := repo.Uow.GetDb().
		Table(entities.InspecaoEntradaExecutadoWeb{}.TableName()).
		Where(&entities.InspecaoEntradaExecutadoWeb{
			RecnoInspecaoEntrada: recnoInspecao,
		}).Find(&result)

	if res.Error != nil {
		return nil, res.Error
	}

	return result, res.Error
}

func (repo *InspecaoEntradaExecutadoWebRepository) InserirInspecaoEntradaExecutadoWeb(inspecaoExecutadaWeb *entities.InspecaoEntradaExecutadoWeb) error {
	err := repo.Uow.GetDb().Where("RECNO_INSPECAO_Entrada = ? AND ESTORNO = ?", inspecaoExecutadaWeb.RecnoInspecaoEntrada, inspecaoExecutadaWeb.Estorno).
		Delete(&entities.InspecaoEntradaExecutadoWeb{}).Error
	if err != nil {
		return err
	}

	return repo.Uow.GetDb().Create(&entities.InspecaoEntradaExecutadoWeb{
		Id:                    utils.NewGuidAsString(),
		RecnoInspecaoEntrada:  inspecaoExecutadaWeb.RecnoInspecaoEntrada,
		IdInspecaoEntradaSaga: inspecaoExecutadaWeb.IdInspecaoEntradaSaga,
		Estorno:               inspecaoExecutadaWeb.Estorno,
		CodigoProduto:         inspecaoExecutadaWeb.CodigoProduto,
		IdRnc:                 inspecaoExecutadaWeb.IdRnc,
	}).Error
}

func (repo *InspecaoEntradaExecutadoWebRepository) RemoverSaga(idSaga string) error {
	var result *entities.InspecaoEntradaExecutadoWeb

	err := repo.Uow.GetDb().
		Table(entities.InspecaoEntradaExecutadoWeb{}.TableName()).
		Where(&entities.InspecaoEntradaExecutadoWeb{
			IdInspecaoEntradaSaga: idSaga,
		}).Find(&result).Error
	if err != nil {
		return err
	}

	if result.Estorno {
		return repo.Uow.GetDb().Where(&entities.InspecaoEntradaExecutadoWeb{IdInspecaoEntradaSaga: idSaga}).Update("ESTORNO", 0).Error
	}

	return repo.Uow.GetDb().Where(&entities.InspecaoEntradaExecutadoWeb{IdInspecaoEntradaSaga: idSaga}).Delete(&entities.InspecaoEntradaExecutadoWeb{}).Error
}
