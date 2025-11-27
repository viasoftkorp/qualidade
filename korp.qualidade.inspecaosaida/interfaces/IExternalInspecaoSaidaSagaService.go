package interfaces

import "bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"

//go:generate mockgen -destination=../mocks/mock_ExternalInspecaoSaidaSagaService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IExternalInspecaoSaidaSagaService
type IExternalInspecaoSaidaSagaService interface {
	PublicarSaga(body interface{}) (string, error)
	BuscarSaga(id string) (*dto.SagaInspecaoSaidaOutput, error)
	BuscarSagas(skip, pageSize int, filters *dto.ProcessamentoInspecaoSaidaFilters, estorno bool) (*dto.GetAllSagaInspecaoSaidaOutput, error)
	ReprocessarSaga(idSaga string) error
	RemoverSaga(idSaga string) error
}
