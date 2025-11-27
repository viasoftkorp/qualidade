package dto

type EstoqueLocalPedidoVendoTotalizacaoDTO struct {
	QuantidadeTotalAlocada float64 `json:"quantidadeTotalAlocada"`
	QuantidadeReprovada    float64 `json:"quantidadeReprovada"`
	QuantidadeAprovada     float64 `json:"quantidadeAprovada"`
}
