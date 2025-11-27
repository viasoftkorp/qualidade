package interfaces

//go:generate mockgen -destination=../mocks/mock_ParametroRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IParametroRepository
type IParametroRepository interface {
	BuscarParametroBool(parametro, secao string) (bool, error)
}
