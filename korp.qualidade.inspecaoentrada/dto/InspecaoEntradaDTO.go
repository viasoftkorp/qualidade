package dto

import (
	"time"
)

type InspecaoEntradaDTO struct {
	RecnoInspecao       int        `json:"recnoInspecao,omitempty"`
	CodigoInspecao      int        `json:"codigoInspecao,omitempty"`
	NotaFiscal          int        `json:"notaFiscal,omitempty"`
	DataInspecao        *time.Time `json:"dataInspecao,omitempty"`
	Inspetor            string     `json:"inspetor,omitempty"`
	Resultado           string     `json:"resultado,omitempty"`
	QuantidadeInspecao  float64    `json:"quantidadeInspecao,omitempty"`
	QuantidadeLote      float64    `json:"quantidadeLote,omitempty"`
	QuantidadeAceita    float64    `json:"quantidadeAceita,omitempty"`
	QuantidadeAprovada  float64    `json:"quantidadeAprovada,omitempty"`
	QuantidadeReprovada float64    `json:"quantidadeReprovada,omitempty"`
	Observacao          string     `json:"observacao,omitempty"`
}

type GetInspecaoEntradaDTO struct {
	Items      []InspecaoEntradaDTO `json:"items"`
	TotalCount int64                `json:"totalCount"`
}

type InspecaoEntradaFilters struct {
	ObservacoesMetricas []string `json:"observacoesMetricas"`
}
