package dto

type AtualizarInspecaoInput struct {
	CodigoInspecao     int                      `json:"codigoInspecao"`
	Itens              []InspecaoEntradaItemDTO `json:"itens"`
	QuantidadeInspecao float64                  `json:"quantidadeInspecao"`
}
