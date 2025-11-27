package models

import "github.com/google/uuid"

type PlanoInspecao struct {
	Id                     uuid.UUID
	CodigoProduto          string
	Sequencia              string
	Descricao              string
	Resultado              string
	MaiorValorInspecionado float64
	MenorValorInspecionado float64
	MaiorValorBase         float64
	MenorValorBase         float64
	Metodo                 string
}
