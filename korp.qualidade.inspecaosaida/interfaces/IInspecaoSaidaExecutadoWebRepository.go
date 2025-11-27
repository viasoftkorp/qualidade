package interfaces

import "bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"

//go:generate mockgen -destination=../mocks/mock_InspecaoSaidaExecutadoWebRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IInspecaoSaidaExecutadoWebRepository
type IInspecaoSaidaExecutadoWebRepository interface {
	BuscarInspecaoSaidaExecutadoWeb(recnoInspecao int) (*entities.InspecaoSaidaExecutadoWeb, error)
	InserirInspecaoSaidaExecutadoWeb(inspecaoSaidaExecutadaWeb *entities.InspecaoSaidaExecutadoWeb) error
	RemoverSaga(idInspecaoSaga string) error
}
