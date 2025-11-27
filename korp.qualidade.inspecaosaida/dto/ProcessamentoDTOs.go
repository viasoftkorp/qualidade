package dto

import "time"

type GetAllProcessamentoInspecaoSaidaOutput struct {
	TotalCount int                                `json:"totalCount"`
	Items      []ProcessamentoInspecaoSaidaOutput `json:"items"`
}

type ProcessamentoInspecaoSaidaOutput struct {
	IdSaga              string                                          `json:"idSaga"`
	Status              int                                             `json:"status"`
	Erro                string                                          `json:"erro"`
	NumeroRetentativas  int                                             `json:"numeroRetentativas"`
	NumeroExecucoes     int                                             `json:"numeroExecucoes"`
	QuantidadeTotal     float64                                         `json:"quantidadeTotal"`
	Resultado           string                                          `json:"resultado"`
	CodigoProduto       string                                          `json:"codigoProduto"`
	DescricaoProduto    string                                          `json:"descricaoProduto"`
	Odf                 int                                             `json:"odf"`
	IdUsuarioExecucao   string                                          `json:"idUsuarioExecucao"`
	NomeUsuarioExecucao string                                          `json:"nomeUsuarioExecucao"`
	DataExecucao        *time.Time                                      `json:"dataExecucao"`
	Lote                string                                          `json:"lote"`
	Estorno             bool                                            `json:"estorno"`
	Transferencias      []ProcessamentoInspecaoSaidaTransferenciaOutput `json:"transferencias"`
	OdfRetrabalho       *int                                            `json:"odfRetrabalho"`
}

type ProcessamentoInspecaoSaidaTransferenciaOutput struct {
	OrdemFabricacao       int     `json:"ordemFabricacao"`
	Quantidade            float64 `json:"quantidade"`
	NumeroPedido          string  `json:"numeroPedido"`
	LocalOrigem           int     `json:"localOrigem"`
	DescricaoLocalOrigem  string  `json:"descricaoLocalOrigem"`
	LocalDestino          int     `json:"localDestino"`
	DescricaoLocalDestino string  `json:"descricaoLocalDestino"`
	TipoTransferencia     int     `json:"tipoTransferencia"`
}

type ProcessamentoInspecaoSaidaFilters struct {
	Status            *int       `json:"status"`
	Resultado         *string    `json:"resultado"`
	QuantidadeTotal   *float64   `json:"quantidadeTotal"`
	CodigoProduto     *string    `json:"codigoProduto"`
	Odf               *int       `json:"odf"`
	Erro              *string    `json:"erro"`
	NumeroExecucoes   *int       `json:"numeroExecucoes"`
	IdUsuarioExecucao *string    `json:"idUsuarioExecucao"`
	DataExecucao      *time.Time `json:"dataExecucao"`
}
