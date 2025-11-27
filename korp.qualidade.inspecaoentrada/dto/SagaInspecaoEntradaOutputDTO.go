package dto

import (
	"time"
)

type GetAllSagaInspecaoEntradaOutput struct {
	TotalCount int                         `json:"totalCount"`
	Items      []SagaInspecaoEntradaOutput `json:"items"`
}

type SagaInspecaoEntradaOutput struct {
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
	NotaFiscal          int
	Lote                string
	Estorno             bool
	RecnoInspecao       int
	IdRnc               *string
	Transferencias      []SagaInspecaoEntradaTransferenciaOutput
}

type SagaInspecaoEntradaTransferenciaOutput struct {
	NumeroPedido       string
	Quantidade         float64
	LocalDestino       int
	LocalOrigem        int
	Lote               string
	LoteOrigem         string
	TipoTransferencia  int
	OrdemFabricacao    int
	Sequencial         int
	SeriesProducao     []InspecaoEntradaSerieProducaoBackgroundOutputDto
	Dimensao1          float64
	Dimensao2          float64
	Dimensao3          float64
	DimensaoDiferenca  float64
	DimensaoQuantidade int
}

type InspecaoEntradaSerieProducaoBackgroundOutputDto struct {
	Serie      string
	RecnoSerie int
}
