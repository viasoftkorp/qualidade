package interfaces

type IParametroService interface {
	BuscarParametroBool(parametro, secao string) (bool, error)
}
