package models

import (
	"github.com/shopspring/decimal"
)

type InspecaoSaida struct {
	CodigoInspecao         int
	Cliente                string
	Pedido                 string
	Odf                    int
	DataInspecao           string
	Inspetor               string
	QtdInspecao            decimal.Decimal
	QtdLote                decimal.Decimal
	IdEmpresa              int
	Recno                  int
	Lote                   string
	Resultado              string
	QuantidadeAceita       decimal.Decimal
	QuantidadeRetrabalhada decimal.Decimal
	QuantidadeAprovada     decimal.Decimal
	QuantidadeReprovada    decimal.Decimal
	CodigoProduto          string
	QuantidadeOrdem        decimal.Decimal
	NumeroPedido           string
}
