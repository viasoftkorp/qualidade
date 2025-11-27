package models

import "github.com/shopspring/decimal"

type Pacote struct {
	NumeroPacote string
	Quantidade   decimal.Decimal
}
