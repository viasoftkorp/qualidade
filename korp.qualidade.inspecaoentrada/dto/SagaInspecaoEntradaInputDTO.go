package dto

import (
	"time"
)

type MovimentarEstoqueInspecaoBackgroundInputDto struct {
	IdSaga             string
	Lote               string
	Estorno            bool
	RecnoInspecao      int
	CodigoProduto      string
	Resultado          string
	OrigemMovimentacao string
	NotaFiscal         int
	Transferencias     []InspecaoEntradaTransferenciaBackgroundInputDto
	IdRnc              *string
}

type InspecaoEntradaTransferenciaBackgroundInputDto struct {
	NumeroPedido      string
	OrdemFabricacao   int
	Fator             int
	Quantidade        float64
	LocalOrigem       int
	LocalDestino      int
	Documento         string
	TipoTransferencia int
	PesoLiquido       *float64
	PesoBruto         *float64
	DataValidade      *time.Time
	DataFabricacao    *time.Time
	UltimoValorPago   float64
	Sequencial        int
	SeriesProducao    []InspecaoEntradaSerieProducaoBackgroundInputDto
}

type InspecaoEntradaSerieProducaoBackgroundInputDto struct {
	RecnoSerie int
	Serie      string
}
