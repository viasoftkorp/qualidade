package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoSaidaSagaService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IInspecaoSaidaSagaService

type IInspecaoSaidaSagaService interface {
	PublicarSagaInspecaoSaida(input *dto.FinalizarInspecaoInput) (bool, *dto.ValidacaoDTO, error)
	PublicarSagaInspecaoSaidaEstorno(inspecaoSaidaRecno int) (bool, *dto.ValidacaoDTO, error)
	BuscarSagas(baseFilters *models.BaseFilter, filters *dto.ProcessamentoInspecaoSaidaFilters, estorno bool) (*dto.GetAllProcessamentoInspecaoSaidaOutput, error)
	ReprocessarSaga(id string) error
	RemoverSaga(id string) error
}
