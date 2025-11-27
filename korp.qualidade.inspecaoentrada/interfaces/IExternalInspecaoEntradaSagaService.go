package interfaces

import "bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"

//go:generate mockgen -destination=../mocks/mock_ExternalInspecaoEntradaSagaService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IExternalInspecaoEntradaSagaService

type IExternalInspecaoEntradaSagaService interface {
	PublicarSaga(body interface{}) (string, error)
	BuscarSaga(id string) (*dto.SagaInspecaoEntradaOutput, error)
	BuscarSagas(skip, pageSize int, filters *dto.ProcessamentoInspecaoEntradaFilters, estorno bool) (*dto.GetAllSagaInspecaoEntradaOutput, error)
	ReprocessarSaga(idSaga string) error
	RemoverSaga(idSaga string) error
}
