package dto

import "time"

type NotaFiscalDTO struct {
	NotaFiscal             int        `json:"notaFiscal"`
	Lote                   string     `json:"lote"`
	Plano                  int        `json:"plano"`
	DescricaoForneced      string     `json:"descricaoForneced"`
	CodigoProduto          string     `json:"codigoProduto"`
	DescricaoProduto       string     `json:"descricaoProduto"`
	DataEntrada            *time.Time `json:"dataEntrada,omitempty"`
	Quantidade             float64    `json:"quantidade"`
	QuantidadeInspecionada float64    `json:"quantidadeInspecionada"`
	QuantidadeInspecionar  float64    `json:"quantidadeInspecionar"`
	RecnoRateioLote        int        `json:"recnoRateioLote"`
	DescricaoPlano         string     `json:"descricaoPlano"`
}

type GetNotasFiscaisOutput struct {
	Items      []NotaFiscalDTO `json:"items"`
	TotalCount int64           `json:"totalCount"`
}

type NotaFiscalFilters struct {
	NotaFiscal    *int       `json:"notaFiscal"`
	Lote          *string    `json:"lote"`
	CodigoProduto *string    `json:"codigoProduto"`
	Fornecedor    *string    `json:"fornecedor"`
	DataEntrada   *time.Time `json:"dataEntrega"`
}
