package dto

type NovaInspecaoInput struct {
	Odf            int                 `json:"odf"`
	CodProduto     string              `json:"codProduto"`
	Plano          string              `json:"plano"`
	Quantidade     float64             `json:"quantidade"`
	Lote           string              `json:"lote"`
	PlanosInspecao []*PlanoInspecaoDTO `json:"planosInspecao"`
}
