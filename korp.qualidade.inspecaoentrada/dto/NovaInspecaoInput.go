package dto

type NovaInspecaoInput struct {
	NotaFiscal     int                 `json:"notaFiscal"`
	Lote           string              `json:"lote"`
	CodigoProduto  string              `json:"codigoProduto"`
	Plano          int                 `json:"plano"`
	Quantidade     float64             `json:"quantidade"`
	PlanosInspecao []*PlanoInspecaoDTO `json:"planosInspecao"`
}
