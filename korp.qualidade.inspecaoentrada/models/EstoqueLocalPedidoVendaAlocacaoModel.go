package models

import "github.com/shopspring/decimal"

type EstoqueLocalPedidoVendaAlocacaoModel struct {
	NumeroPedido                  string
	NumeroOdf                     int
	QuantidadeTotalPedido         decimal.Decimal
	QuantidadeAlocadaLoteLocal    decimal.Decimal
	QuantidadeEntrada             decimal.Decimal
	QuantidadeRestanteInspecionar decimal.Decimal
	DescricaoProduto              string
	DescricaoLocalReprovado       string
	DescricaoLocalAprovado        string
	CodigoLocalReprovado          int
	CodigoLocalAprovado           int
	QuantidadeAprovada            decimal.Decimal
	QuantidadeReprovada           decimal.Decimal
}
