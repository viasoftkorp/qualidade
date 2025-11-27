package models

import "github.com/shopspring/decimal"

type InspecaoSaidaJoin struct {
	Recno                  int
	CodigoInspecao         int
	Odf                    int
	Resultado              string
	QtdLote                decimal.Decimal
	CodigoLocalAprovado    int
	QuantidadeAprovada     decimal.Decimal
	CodigoLocalReprovado   int
	QuantidadeReprovada    decimal.Decimal
	CodigoLocalRetrabalho  int
	QuantidadeRetrabalhada decimal.Decimal
}
