package dto

type InspecaoEntradaItemDTO struct {
	Id                     string  `json:"id"`
	CodigoProduto          int     `json:"codigoProduto"`
	Descricao              string  `json:"descricao"`
	Metodo                 string  `json:"metodo"`
	Sequencia              string  `json:"sequencia"`
	Resultado              string  `json:"resultado,omitempty"`
	MaiorValorInspecionado float64 `json:"maiorValorInspecionado,omitempty"`
	MenorValorInspecionado float64 `json:"menorValorInspecionado,omitempty"`
	MaiorValorBase         float64 `json:"maiorValorBase,omitempty"`
	MenorValorBase         float64 `json:"menorValorBase,omitempty"`
	Observacao             string  `json:"observacao"`
}

type GetInspecaoEntradaItensDTO struct {
	Items      []InspecaoEntradaItemDTO `json:"items"`
	TotalCount int64                    `json:"totalCount"`
}
