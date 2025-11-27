package models

type NotaFiscal struct {
	Nota               int     `json:"nota"`
	NumeroNota         int     `json:"numeroNota"`
	ClienteCodigo      string  `json:"clienteCodigo"`
	ClienteRazaoSocial string  `json:"clienteRazaoSocial"`
	QuantidadeLote     float64 `json:"quantidadeLote"`
}
