package models

import (
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type InspecaoSaida struct {
	Id                     uuid.UUID       `json:"id"`
	CodigoInspecao         int             `json:"codigoInspecao"`
	Cliente                string          `json:"cliente"`
	Pedido                 string          `json:"pedido"`
	Odf                    int             `json:"odf"`
	OdfApontada            int             `json:"odfApontada"`
	DataInspecao           string          `json:"dataInspecao"`
	Inspetor               string          `json:"inspetor"`
	QtdInspecao            decimal.Decimal `json:"qtdInspecao"`
	QtdLote                decimal.Decimal `json:"qtdLote"`
	IdEmpresa              int             `json:"idEmpresa"`
	Recno                  int             `json:"recno"`
	Lote                   string          `json:"lote"`
	Resultado              string          `json:"resultado"`
	QuantidadeAceita       decimal.Decimal `json:"quantidadeAceita"`
	QuantidadeRetrabalhada decimal.Decimal `json:"quantidadeRetrabalhada"`
	QuantidadeAprovada     decimal.Decimal `json:"quantidadeAprovada"`
	QuantidadeReprovada    decimal.Decimal `json:"quantidadeReprovada"`
	CodigoProduto          string          `json:"codigoProduto"`
	QuantidadeOrdem        decimal.Decimal `json:"quantidadeOrdem"`
	NumeroPedido           string          `json:"numeroPedido"`
}
