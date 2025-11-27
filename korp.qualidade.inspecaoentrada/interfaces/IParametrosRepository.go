package interfaces

//go:generate mockgen -destination=../mocks/mock_ParametrosRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IParametrosRepository
type IParametrosRepository interface {
	BuscarValorParametro(chaveParametro, secao string) (bool, error)
}
