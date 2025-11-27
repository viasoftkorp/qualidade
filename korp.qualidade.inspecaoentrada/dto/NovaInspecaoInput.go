package dto

type NovaInspecaoInput struct {
    RecnoItemNotaFiscal int             `json:"recnoItemNotaFiscal"`
	NotaFiscal      int                 `json:"notaFiscal"`
	Lote            string              `json:"lote"`
	CodigoProduto   string              `json:"codigoProduto"`
	Plano           string              `json:"plano"`
	Quantidade      float64             `json:"quantidade"`
	PlanosInspecao  []*PlanoInspecaoDTO `json:"planosInspecao"`
	SerieNotaFiscal string              `json:"serieNotaFiscal"`
}
