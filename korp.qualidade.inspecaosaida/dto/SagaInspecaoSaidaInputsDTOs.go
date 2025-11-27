package dto

import "time"

type MovimentarEstoqueInspecaoBackgroundInputDto struct {
	IdSaga             string
	Lote               string
	Estorno            bool
	RecnoInspecao      int
	CodigoProduto      string
	Resultado          string
	OrigemMovimentacao string
	OrdemFabricacao    int
	Transferencias     []InspecaoSaidaTransferenciaBackgroundInputDto
	OrdemRetrabalho    *InspecaoSaidaOrdemRetrabalhoBackgroundDto
	IdRnc              *string
	CodigoRnc          *int
}

type InspecaoSaidaTransferenciaBackgroundInputDto struct {
	NumeroPedido               string
	Fator                      int
	Quantidade                 float64
	LocalOrigem                int
	LocalDestino               int
	Documento                  string
	TipoTransferencia          int
	PesoLiquido                *float64
	PesoBruto                  *float64
	DataValidade               *time.Time
	DataFabricacao             *time.Time
	UltimoValorPago            float64
	OrdemFabricacao            int
	Sequencial                 int
	LegacyIdProcessoEngenharia int
	SeriesProducao             []InspecaoSaidaSerieProducaoBackgroundInputDto
}

type InspecaoSaidaSerieProducaoBackgroundInputDto struct {
	Serie      string
	RecnoSerie int
}
