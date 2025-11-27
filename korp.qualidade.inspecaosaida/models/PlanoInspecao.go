package models

import "github.com/google/uuid"

type PlanoInspecao struct {
	Id             uuid.UUID
	CodProduto     string
	Sequencia      string
	Descricao      string
	Resultado      string
	MaiorValor     float64
	MenorValor     float64
	MaiorValorBase float64
	MenorValorBase float64
	Metodo         string
}
