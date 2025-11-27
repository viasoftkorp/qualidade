package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
)

type EngenhariaRepository struct {
	interfaces.IEngenhariaRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewEngenhariaRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (interfaces.IEngenhariaRepository, error) {
	return &EngenhariaRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *EngenhariaRepository) BuscarProcesso(codigoProduto string) (*dto.ProcessoEngenhariaOutput, error) {
	var processo dto.ProcessoEngenhariaOutput

	err := repo.Uow.GetDb().
		Table("PROCESSO").
		Select("PROCESSO.NUMPEC AS CodigoProduto, LOCAIS.CODIGO AS CodigoLocalDestino, LOCAIS.DESCRICAO AS DescricaoLocalDestino").
		Joins("LEFT JOIN LOCAIS ON LOCAIS.R_E_C_N_O_ = PROCESSO.LOCAL_DESTINO").
		Where("PROCESSO.ATIVO = 'S' AND PROCESSO.NUMPEC = ?", codigoProduto).
		Scan(&processo).
		Error

	if err != nil {
		return nil, err
	}

	return &processo, nil
}
