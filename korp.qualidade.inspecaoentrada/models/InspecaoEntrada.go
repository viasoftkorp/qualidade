package models

import "github.com/shopspring/decimal"

type InspecaoEntrada struct {
	Recno               int
	CodigoInspecao      int
	Fornecedor          string
	NotaFiscal          int
	RecnoRateio         int
	DataInspecao        string
	Inspetor            string
	Lote                string
	QuantidadeAceita    decimal.Decimal
	QuantidadeAprovada  decimal.Decimal
	QuantidadeReprovada decimal.Decimal
	QuantidadeInspecao  decimal.Decimal
	QuantidadeLote      decimal.Decimal
	Resultado           string
	IdEmpresa           int
}
