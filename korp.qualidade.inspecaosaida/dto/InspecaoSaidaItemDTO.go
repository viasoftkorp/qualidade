package dto

type InspecaoSaidaItemDTO struct {
	Id                     string  `json:"id"`
	LegacyIdPlanoInspecao  int     `json:"LegacyIdPlanoInspecao"`
	Plano                  string  `json:"plano"`
	Odf                    int     `json:"odf"`
	Descricao              string  `json:"descricao"`
	Metodo                 string  `json:"metodo"`
	Sequencia              string  `json:"sequencia"`
	Resultado              string  `json:"resultado,omitempty"`
	MaiorValor             float64 `json:"maiorValor"`
	MenorValor             float64 `json:"menorValor"`
	MaiorValorBase         float64 `json:"maiorValorBase"`
	MenorValorBase         float64 `json:"menorValorBase"`
	Observacao             string  `json:"observacao"`
}

type GetInspecaoSaidaItensDTO struct {
	Items      []*InspecaoSaidaItemDTO `json:"items"`
	TotalCount int64                   `json:"totalCount"`
}
