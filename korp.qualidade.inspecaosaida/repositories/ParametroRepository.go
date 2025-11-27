package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"context"
	"time"
)

type ParametroRepository struct {
	interfaces.IParametroRepository
	Uow unit_of_work.UnitOfWork
}

func NewParametroRepository(uow unit_of_work.UnitOfWork) interfaces.IParametroRepository {
	return &ParametroRepository{
		Uow: uow,
	}
}

func (repository *ParametroRepository) BuscarParametroBool(parametro, secao string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	BooleanParam := "F"
	res := repository.Uow.GetDb().WithContext(ctx).
		Raw("SELECT VALOR AS BooleanParam FROM PARAMETROS WHERE CHAVE = ? AND SECAO = ?", parametro, secao).
		First(&BooleanParam)
	if res.Error != nil {
		return false, res.Error
	}

	return BooleanParam == "T", nil
}
