package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
)

type EmpresaRepository struct {
	interfaces.IEmpresaRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewEmpresaRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (interfaces.IEmpresaRepository, error) {
	return &EmpresaRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *EmpresaRepository) BuscarLogo(legacyIdEmpresa int) ([]byte, error) {
	var logo *string

	res := repo.Uow.GetDb().Raw("SELECT LOGO FROM EMPRESA WHERE R_E_C_N_O_ = ?", legacyIdEmpresa).
		Scan(&logo)

	if logo == nil {
		return nil, res.Error
	}

	return []byte(*logo), res.Error
}
