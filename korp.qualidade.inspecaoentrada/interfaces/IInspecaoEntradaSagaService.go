package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoEntradaSagaService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IInspecaoEntradaSagaService

type IInspecaoEntradaSagaService interface {
	PublicarSagaInspecaoEntrada(input *dto.FinalizarInspecaoInput) (bool, *dto.ValidacaoDTO, error)
	PublicarSagaInspecaoEntradaEstorno(inspecaoEntradaRecno int) (bool, *dto.ValidacaoDTO, error)
	BuscarSagas(baseFilters *models.BaseFilter, filters *dto.ProcessamentoInspecaoEntradaFilters, estorno bool) (*dto.GetAllProcessamentoInspecaoEntradaOutput, error)
	ReprocessarSaga(id string) error
	RemoverSaga(id string) error
}
