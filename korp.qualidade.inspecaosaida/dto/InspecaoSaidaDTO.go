package dto

import (
	"time"
)

type InspecaoSaidaDTO struct {
	CodigoInspecao         int        `json:"codigoInspecao,omitempty"`
	ODF                    int        `json:"odf,omitempty"`
	DataInspecao           *time.Time `json:"dataInspecao,omitempty"`
	Inspetor               string     `json:"inspetor,omitempty"`
	Resultado              string     `json:"resultado,omitempty"`
	QuantidadeInspecao     float64    `json:"quantidadeInspecao,omitempty"`
	QuantidadeLote         float64    `json:"quantidadeLote,omitempty"`
	QuantidadeAceita       float64    `json:"quantidadeAceita,omitempty"`
	QuantidadeRetrabalhada float64    `json:"quantidadeRetrabalhada,omitempty"`
	QuantidadeAprovada     float64    `json:"quantidadeAprovada,omitempty"`
	QuantidadeReprovada    float64    `json:"quantidadeReprovada,omitempty"`
	Lote                   string     `json:"lote"`
	RecnoInspecaoSaida     int        `json:"recnoInspecaoSaida"`
	CodigoProduto          string     `json:"codigoProduto"`
	QuantidadeOrdem        float64    `json:"quantidadeOrdem"`
	NumeroPedido           string     `json:"numeroPedido"`
	Cliente                string     `json:"cliente"`
	TipoInspecao           string     `json:"tipoInspecao"`
}

type GetInspecaoSaidaDTO struct {
	Items      []*InspecaoSaidaDTO `json:"items"`
	TotalCount int64               `json:"totalCount"`
}
