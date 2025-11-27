package dto

type ProcessoEngenhariaOutput struct {
	CodigoProduto         string  `json:"codigoProduto"`
	CodigoLocalDestino    *int    `json:"codigoLocalDestino"`
	DescricaoLocalDestino *string `json:"descricaoLocalDestino"`
}
