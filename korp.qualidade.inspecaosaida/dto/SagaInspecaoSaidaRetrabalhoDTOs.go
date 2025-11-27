package dto

import "time"

type InspecaoSaidaOrdemRetrabalhoBackgroundDto struct {
	IdRnc                 string
	Quantidade            float64
	CodigoProduto         string
	CodigoCliente         string
	DataEntrega           *time.Time
	NumeroPedido          string
	OrdemFabricacaoOrigem int
	OrdemFabricacao       *int
	Retrabalho            bool
	Materias              []InspecaoSaidaOrdemRetrabalhoMaterialackgroundDto
	Maquinas              []InspecaoSaidaOrdemRetrabalhoMaquinaBackgroundDto
}

type InspecaoSaidaOrdemRetrabalhoMaquinaBackgroundDto struct {
	RecnoMaquina      int
	QuantidadeHoras   float64
	Operacao          string
	Sequencia         string
	DescricaoOperacao string
}

type InspecaoSaidaOrdemRetrabalhoMaterialackgroundDto struct {
	Quantidade      float64
	CodigoProduto   string
	Operacao        string
	CodigoCategoria string
	Sequencia       string
}
