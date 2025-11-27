package models

import (
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type InspecaoEntradaItem struct {
	Id                     uuid.UUID
	Plano                  int
	Descricao              string
	Metodo                 string
	Sequencia              string
	Resultado              string
	MaiorValorInspecionado decimal.Decimal
	MenorValorInspecionado decimal.Decimal
	MaiorValorBase         decimal.Decimal
	MenorValorBase         decimal.Decimal
	CodigoInspecao         int
	Observacao             string
}
