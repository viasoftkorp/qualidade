package models

import "github.com/shopspring/decimal"

type OrdemProducao struct {
	RecnoPpedlise          int
	ODF                    int
	OdfApontada            int
	CodigoProduto          string
	DescricaoProduto       string
	Situacao               string
	Revisao                string
	QuantidadeOrdem        decimal.Decimal
	QuantidadeProduzida    decimal.Decimal
	QuantidadeInspecionada decimal.Decimal
	QuantidadeLote         decimal.Decimal
	DataInicio             string
	DataEntrega            string
	DataEmissao            string
	Lote                   string
	NumeroPedido           string
	Cliente                string
	Plano                  string
	DataNegociada          string
	DescricaoPlano         string
	RecnoProcesso          int
}
