package dto

type PlanoInspecaoDTO struct {
	Id             string  `json:"id"`
	Descricao      string  `json:"descricao"`
	Resultado      string  `json:"resultado,omitempty"`
	MaiorValor     float64 `json:"maiorValor,omitempty"`
	MenorValor     float64 `json:"menorValor,omitempty"`
	MaiorValorBase float64 `json:"maiorValorBase"`
	MenorValorBase float64 `json:"menorValorBase"`
	Metodo         string  `json:"metodo"`
	Observacao     string  `json:"observacao"`
}

type GetPlanosInspecaoDTO struct {
	Items      []*PlanoInspecaoDTO `json:"items"`
	TotalCount int64               `json:"totalCount"`
}
