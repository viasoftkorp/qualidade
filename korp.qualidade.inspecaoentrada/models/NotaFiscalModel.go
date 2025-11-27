package models

import "github.com/shopspring/decimal"

type NotaFiscalModel struct {
	IdNotaFiscal           int             `json:"idNotaFiscal"`
	NotaFiscal             int             `json:"notaFiscal"`
	Plano                  int             `json:"plano"`
	Lote                   string          `json:"lote"`
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
}

type GetNotasFiscaisOutput struct {
	Items      []NotaFiscalModel `json:"items"`
	TotalCount int64             `json:"totalCount"`
}
