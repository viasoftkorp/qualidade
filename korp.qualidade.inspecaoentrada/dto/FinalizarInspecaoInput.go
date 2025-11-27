package dto

type FinalizarInspecaoInput struct {
	Lote                 string                `json:"lote"`
	CodigoInspecao       int                   `json:"codigoInspecao"`
	Resultado            string                `json:"resultado"`
	QuantidadeAprovada   float64               `json:"quantidadeAprovada"`
	QuantidadeRejeitada  float64               `json:"quantidadeRejeitada"`
	CodigoLocalPrincipal int                   `json:"codigoLocalPrincipal"`
	CodigoLocalReprovado int                   `json:"codigoLocalReprovado"`
	IdRnc                *string               `json:"idRnc"`
	Lotes                []*PedidoVendaLoteDto `json:"lotes"`
}
