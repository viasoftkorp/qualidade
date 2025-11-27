package dto

import "time"

type RncDetailsOutputDTO struct {
	IdNotaFiscal        string     `json:"idNotaFiscal"`
	IdFornecedor        string     `json:"idFornecedor"`
	NumeroNota          string     `json:"numeroNota"`
	DataFabricacaoLote  *time.Time `json:"dataFabricacaoLote"`
	QuantidadeTotalLote float64    `json:"quantidadeTotalLote"`
	NumeroLote          string     `json:"numeroLote"`
	IdProduto           string     `json:"idProduto"`
}
