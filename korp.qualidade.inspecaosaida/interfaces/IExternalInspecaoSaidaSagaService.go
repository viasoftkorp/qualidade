package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_ExternalInspecaoSaidaSagaService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IExternalInspecaoSaidaSagaService
type IExternalInspecaoSaidaSagaService interface {
	PublicarSaga(body interface{}) (string, error)
	BuscarSaga(id string) (*dto.SagaInspecaoSaidaOutput, error)
	BuscarSagas(baseFilters *models.BaseFilter, filters *dto.ProcessamentoInspecaoSaidaFilters, estorno bool) (*dto.GetAllSagaInspecaoSaidaOutput, error)
	ReprocessarSaga(idSaga string) error
	RemoverSaga(idSaga string) error
}
