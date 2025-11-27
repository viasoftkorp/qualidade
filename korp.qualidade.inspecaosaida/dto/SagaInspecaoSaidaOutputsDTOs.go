package dto

import "time"

type GetAllSagaInspecaoSaidaOutput struct {
	TotalCount int                       `json:"totalCount"`
	Items      []SagaInspecaoSaidaOutput `json:"items"`
}

type SagaInspecaoSaidaOutput struct {
	Id                  string
	Status              int
	Erro                string
	NumeroRetentativas  int
	NumeroExecucoes     int
	IdUsuarioExecucao   string
	NomeUsuarioExecucao string
	DataExecucao        *time.Time
	CodigoProduto       string
	QuantidadeTotal     float64
	Resultado           string
	Lote                string
	Estorno             bool
	RecnoInspecao       int
	OrdemFabricacao     int
	Transferencias      []SagaInspecaoSaidaTransferenciaOutput
	OrdemRetrabalho     *InspecaoSaidaOrdemRetrabalhoBackgroundDto
}

type SagaInspecaoSaidaTransferenciaOutput struct {
	NumeroPedido      string
	Quantidade        float64
	LocalDestino      int
	LocalOrigem       int
	TipoTransferencia int
	OrdemFabricacao   int
	Sequencial        int
	SeriesProducao    []InspecaoSaidaSerieProducaoBackgroundOutputDto
}

type InspecaoSaidaSerieProducaoBackgroundOutputDto struct {
	Serie      string
	RecnoSerie int
}
