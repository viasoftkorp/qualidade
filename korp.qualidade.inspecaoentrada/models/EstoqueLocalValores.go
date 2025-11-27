package models

import (
	"github.com/shopspring/decimal"
)

type EstoqueLocalValores struct {
	Recno          int
	Quantidade     decimal.Decimal
	PesoBruto      *decimal.Decimal
	PesoLiquido    *decimal.Decimal
	DataValidade   string
	DataFabricacao string
	ValorPago      decimal.Decimal
}
