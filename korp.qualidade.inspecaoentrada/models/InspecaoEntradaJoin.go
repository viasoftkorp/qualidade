package models

import "github.com/shopspring/decimal"

type InspecaoEntradaJoin struct {
	Recno                int
	NotaFiscal           int
	NumeroPedido         string
	CodigoInspecao       int
	Odf                  int
	Resultado            string
	Lote                 string
	CodigoLocalAprovado  int
	QuantidadeAprovada   decimal.Decimal
	CodigoLocalReprovado int
	QuantidadeReprovada  decimal.Decimal
	QuantidadeLote       decimal.Decimal
}
