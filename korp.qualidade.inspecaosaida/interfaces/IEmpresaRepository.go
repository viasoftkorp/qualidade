package interfaces

//go:generate mockgen -destination=../mocks/mock_EmpresaRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IEmpresaRepository
type IEmpresaRepository interface {
	BuscarLogo(legacyIdEmpresa int) ([]byte, error)
}
