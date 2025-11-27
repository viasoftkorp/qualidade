package models

import "github.com/shopspring/decimal"

type InspecaoEntrada struct {
	Recno               int
	RecnoItemNotaFiscal int
	CodigoInspecao      int
	CodigoProduto       string
	Fornecedor          string
	NotaFiscal          int
	SerieNotaFiscal     string
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
