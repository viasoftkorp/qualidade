package models

import (
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type NotaFiscalModel struct {
	Id                     uuid.UUID       `json:"id"`
	Recno                  int             `json:"recno"`
	NotaFiscal             int             `json:"notaFiscal"`
	Plano                  string          `json:"plano"`
	Lote                   string          `json:"lote"`
	CodigoForneced         string          `json:"codigoForneced"`
	DescricaoForneced      string          `json:"descricaoForneced"`
	CodigoProduto          string          `json:"codigoProduto"`
	DescricaoProduto       string          `json:"descricaoProduto"`
	DataEntrada            string          `json:"dataEntrada"`
	DataEmissao            string          `json:"dataEmissao"`
	Quantidade             decimal.Decimal `json:"quantidade"`
	QuantidadeInspecionar  decimal.Decimal `json:"quantidadeInspecionar"`
	QuantidadeInspecionada decimal.Decimal `json:"quantidadeInspecionada"`
	RecnoRateioLote        int             `json:"recnoRateioLote"`
	DescricaoPlano         string          `json:"descricaoPlano"`
	CodigoLocal            int             `json:"codigoLocal"`
	Serie                  string          `json:"serie"`
	Observacao             string          `json:"observacao"`
	DataFabricacao         string          `json:"dataFabricacao"`
	DataValidade           string          `json:"dataValidade"`
	IdEmpresa              int             `json:"idEmpresa"`
}

type GetNotasFiscaisOutput struct {
	Items      []NotaFiscalModel `json:"items"`
	TotalCount int64             `json:"totalCount"`
}
