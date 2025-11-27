package dto

type PlanoInspecaoDTO struct {
	Id                     string  `json:"id"`
	LegacyId               int     `json:"legacyId"`
	Descricao              string  `json:"descricao"`
	Resultado              string  `json:"resultado,omitempty"`
	MaiorValorInspecionado float64 `json:"maiorValorInspecionado,omitempty"`
	MenorValorInspecionado float64 `json:"menorValorInspecionado,omitempty"`
	MaiorValorBase         float64 `json:"maiorValorBase"`
	MenorValorBase         float64 `json:"menorValorBase"`
	Metodo                 string  `json:"metodo"`
	Observacao             string  `json:"observacao"`
}

type GetPlanosInspecaoDTO struct {
	Items      []*PlanoInspecaoDTO `json:"items"`
	TotalCount int64               `json:"totalCount"`
}
