package models

import (
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type InspecaoEntradaItem struct {
	Id                     uuid.UUID
	LegacyIdPlanoInspecao  int
	Plano                  string
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
