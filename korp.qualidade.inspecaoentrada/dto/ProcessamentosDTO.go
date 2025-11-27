package dto

import "time"

type GetAllProcessamentoInspecaoEntradaOutput struct {
	TotalCount int                                  `json:"totalCount"`
	Items      []ProcessamentoInspecaoEntradaOutput `json:"items"`
}

type ProcessamentoInspecaoEntradaOutput struct {
	IdSaga              string                                            `json:"idSaga"`
	Status              int                                               `json:"status"`
	Erro                string                                            `json:"erro"`
	NumeroRetentativas  int                                               `json:"numeroRetentativas"`
	NumeroExecucoes     int                                               `json:"numeroExecucoes"`
	QuantidadeTotal     float64                                           `json:"quantidadeTotal"`
	Resultado           string                                            `json:"resultado"`
	CodigoProduto       string                                            `json:"codigoProduto"`
	DescricaoProduto    string                                            `json:"descricaoProduto"`
	NotaFiscal          int                                               `json:"notaFiscal"`
	IdUsuarioExecucao   string                                            `json:"idUsuarioExecucao"`
	NomeUsuarioExecucao string                                            `json:"nomeUsuarioExecucao"`
	DataExecucao        *time.Time                                        `json:"dataExecucao"`
	Lote                string                                            `json:"lote"`
	Estorno             bool                                              `json:"estorno"`
	Transferencias      []ProcessamentoInspecaoEntradaTransferenciaOutput `json:"transferencias"`
}

type ProcessamentoInspecaoEntradaTransferenciaOutput struct {
	Quantidade            float64 `json:"quantidade"`
	NumeroPedido          string  `json:"numeroPedido"`
	LocalOrigem           int     `json:"localOrigem"`
	DescricaoLocalOrigem  string  `json:"descricaoLocalOrigem"`
	LocalDestino          int     `json:"localDestino"`
	DescricaoLocalDestino string  `json:"descricaoLocalDestino"`
	TipoTransferencia     int     `json:"tipoTransferencia"`
	Lote                  string  `json:"lote"`
	LoteOrigem            string  `json:"loteOrigem"`
}

type ProcessamentoInspecaoEntradaFilters struct {
	Status            *int       `json:"status"`
	Resultado         *string    `json:"resultado"`
	QuantidadeTotal   *float64   `json:"quantidadeTotal"`
	CodigoProduto     *string    `json:"codigoProduto"`
	NotaFiscal        *int       `json:"notaFiscal"`
	Erro              *string    `json:"erro"`
	NumeroExecucoes   *int       `json:"numeroExecucoes"`
	IdUsuarioExecucao *string    `json:"idUsuarioExecucao"`
	DataExecucao      *time.Time `json:"dataExecucao"`
}
