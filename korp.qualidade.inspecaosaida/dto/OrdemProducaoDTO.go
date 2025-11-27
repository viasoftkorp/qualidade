package dto

import (
	"time"
)

type OrdemProducaoDTO struct {
	ODF                    int        `json:"odf"`
	OdfApontada            int        `json:"odfApontada"`
	Lote                   string     `json:"lote"`
	CodigoProduto          string     `json:"codigoProduto"`
	DescricaoProduto       string     `json:"descricaoProduto"`
	Situacao               string     `json:"situacao"`
	Revisao                string     `json:"revisao"`
	QuantidadeOrdem        float64    `json:"quantidadeOrdem"`
	QuantidadeProduzida    float64    `json:"quantidadeProduzida"`
	QuantidadeInspecionada float64    `json:"quantidadeInspecionada"`
	QuantidadeInspecionar  float64    `json:"quantidadeInspecionar"`
	DataInicio             *time.Time `json:"dataInicio,omitempty"`
	DataEntrega            *time.Time `json:"dataEntrega,omitempty"`
	DataEmissao            *time.Time `json:"dataEmissao,omitempty"`
	Plano                  string     `json:"plano"`
	DataNegociada          string     `json:"dataNegociada"`
	DescricaoPlano         string     `json:"descricaoPlano"`
	NumeroPedido           string     `json:"numeroPedido"`
	Cliente                string     `json:"cliente"`
	RecnoProcesso          int        `json:"recnoProcesso"`
}

type GetOrdemProducaoDTO struct {
	Items      []OrdemProducaoDTO `json:"items"`
	TotalCount int64              `json:"totalCount"`
}

type OrdemProducaoFilters struct {
	Odf                 *int       `json:"odf"`
	Lote                *string    `json:"lote"`
	CodigoProduto       *string    `json:"codigoProduto"`
	DataInicio          *time.Time `json:"dataInicio"`
	DataEntrega         *time.Time `json:"dataEntrega"`
	DataEmissao         *time.Time `json:"dataEmissao"`
	ObservacoesMetricas []string   `json:"observacoesMetricas"`
}
