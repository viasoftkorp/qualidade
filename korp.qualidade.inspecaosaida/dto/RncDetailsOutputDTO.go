package dto

import "time"

type RncDetailsOutputDTO struct {
	IdCliente           *string    `json:"idCliente"`
	IdProduto           string     `json:"idProduto"`
	Revisao             int        `json:"revisao"`
	DataFabricacaoLote  *time.Time `json:"dataFabricacaoLote"`
	QuantidadeTotalLote float64    `json:"quantidadeTotalLote"`
	NumeroLote          string     `json:"numeroLote"`
	NumeroOdf           string     `json:"numeroOdf"`
}
