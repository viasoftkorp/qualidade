package dto

type AtualizarInspecaoInput struct {
	CodInspecao        int                     `json:"codInspecao"`
	Itens              []*InspecaoSaidaItemDTO `json:"itens"`
	QuantidadeInspecao float64                 `json:"quantidadeInspecao"`
}
