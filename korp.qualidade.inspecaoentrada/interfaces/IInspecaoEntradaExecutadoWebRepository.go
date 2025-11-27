package interfaces

import "bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"

//go:generate mockgen -destination=../mocks/mock_InspecaoEntradaExecutadoWebRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IInspecaoEntradaExecutadoWebRepository

type IInspecaoEntradaExecutadoWebRepository interface {
	BuscarInspecaoEntradaExecutadoWeb(recnoInspecao int) (*entities.InspecaoEntradaExecutadoWeb, error)
	InserirInspecaoEntradaExecutadoWeb(inspecaoExecutadaWeb *entities.InspecaoEntradaExecutadoWeb) error
	RemoverSaga(idInspecaoEntradaSaga string) error
}
