package dto

type PlanoAmostragemDTO struct {
	Id                    string  `json:"id"`
	QuantidadeMinima      float64 `json:"quantidadeMinima"`
	QuantidadeMaxima      float64 `json:"quantidadeMaxima"`
	QuantidadeInspecionar float64 `json:"quantidadeInspecionar"`
}

type GetAllPlanoAmostragemDTO struct {
	Items      []PlanoAmostragemDTO `json:"items"`
	TotalCount int64                `json:"totalCount"`
}
