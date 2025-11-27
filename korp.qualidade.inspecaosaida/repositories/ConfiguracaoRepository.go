package repositories

import (
	"strconv"
	"strings"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
)

type ConfiguracaoRepository struct {
	interfaces.IConfiguracaoRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewConfiguracaoRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (interfaces.IConfiguracaoRepository, error) {
	return &ConfiguracaoRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *ConfiguracaoRepository) GetConfiguracao() (dto.ConfiguracaoDto, error) {
	var configuracao dto.ConfiguracaoDto

	res := repo.Uow.GetDb().Raw(queries.GetConfiguracao).First(&configuracao)

	return configuracao, res.Error
}

func (repo *ConfiguracaoRepository) UpdateConfiguracao(configuracao dto.ConfiguracaoDto) error {
	var query = queries.UpdateConfiguracao

	query = strings.ReplaceAll(query, "@UsarNotaImpressaoRelatorio", strconv.Itoa(btoi(configuracao.UsarNotaImpressaoRelatorio)))
	query = strings.ReplaceAll(query, "@Id", configuracao.Id.String())

	res := repo.Uow.GetDb().Exec(query)

	return res.Error
}

func btoi(b bool) int {
	if b {
		return 1
	}
	return 0
}
