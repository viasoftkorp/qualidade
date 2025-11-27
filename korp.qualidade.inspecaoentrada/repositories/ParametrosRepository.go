package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"context"
	"time"
)

type ParametrosRepository struct {
	interfaces.IParametrosRepository
	Uow unit_of_work.UnitOfWork
}

func NewParametrosRepository(uow unit_of_work.UnitOfWork) (
	interfaces.IParametrosRepository, error) {
	return &ParametrosRepository{
		Uow: uow,
	}, nil
}

func (repo *ParametrosRepository) BuscarValorParametro(chaveParametro, secao string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	BooleanParam := "F"
	res := repo.Uow.GetDb().WithContext(ctx).
		Raw("SELECT VALOR AS BooleanParam FROM PARAMETROS WHERE CHAVE = ? AND SECAO = ?", chaveParametro, secao).
		First(&BooleanParam)
	if res.Error != nil {
		return false, res.Error
	}

	return BooleanParam == "T", nil
}
