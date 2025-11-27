package dto

import "time"

type InspecaoEntradaHistoricoCabecalhoDTO struct {
	NotaFiscal         int     `json:"notaFiscal"`
	CodigoProduto      string  `json:"codigoProduto"`
	DescricaoProduto   string  `json:"descricaoProduto"`
	QuantidadeLote     float64 `json:"quantidadeLote"`
	QuantidadeInspecao float64 `json:"quantidadeInspecao"`
}

type GetAllInspecaoEntradaHistoricoCabecalhoDTO struct {
	Items      []InspecaoEntradaHistoricoCabecalhoDTO `json:"items"`
	TotalCount int64                                  `json:"totalCount"`
}

type InspecaoEntradaHistoricoCabecalhoFilters struct {
	NotaFiscal    *int    `json:"notaFiscal"`
	CodigoProduto *string `json:"codigoProduto"`
	Lote          *string `json:"lote"`
}

type InspecaoEntradaHistoricoItems struct {
	RecnoInspecao       int        `json:"recnoInspecao"`
	NotaFiscal          int        `json:"notaFiscal"`
	CodigoProduto       string     `json:"codigoProduto"`
	DescricaoProduto    string     `json:"descricaoProduto"`
	QuantidadeInspecao  float64    `json:"quantidadeInspecao"`
	QuantidadeAprovada  float64    `json:"quantidadeAprovada"`
	QuantidadeReprovada float64    `json:"quantidadeReprovada"`
	Inspetor            string     `json:"inspetor"`
	Resultado           string     `json:"resultado"`
	DataInspecao        *time.Time `json:"dataInspecao,omitempty"`
	CodigoInspecao      int        `json:"codigoInspecao"`
	IdRnc               *string    `json:"idRnc"`
}

type InspecaoEntradaHistoricoItemsDTO struct {
	RecnoInspecao       int                                           `json:"recnoInspecao"`
	CodigoInspecao      int                                           `json:"codigoInspecao"`
	NotaFiscal          int                                           `json:"ordemFabricacao"`
	CodigoProduto       string                                        `json:"codigoProduto"`
	DescricaoProduto    string                                        `json:"descricaoProduto"`
	QuantidadeInspecao  float64                                       `json:"quantidadeInspecao"`
	QuantidadeAprovada  float64                                       `json:"quantidadeAprovada"`
	QuantidadeReprovada float64                                       `json:"quantidadeReprovada"`
	Inspetor            string                                        `json:"inspetor"`
	Resultado           string                                        `json:"resultado"`
	DataInspecao        *time.Time                                    `json:"dataInspecao,omitempty"`
	Transferencias      []HistoricoInspecaoEntradaTransferenciaOutput `json:"transferencias"`
	IdRnc               *string                                       `json:"idRnc"`
}

type GetAllInspecaoEntradaHistoricoItemsDTO struct {
	Items      []InspecaoEntradaHistoricoItemsDTO `json:"items"`
	TotalCount int64                              `json:"totalCount"`
}

type HistoricoInspecaoEntradaTransferenciaOutput struct {
	NotaFiscal            int     `json:"ordemFabricacao"`
	Quantidade            float64 `json:"quantidade"`
	NumeroPedido          string  `json:"numeroPedido"`
	LocalOrigem           int     `json:"localOrigem"`
	DescricaoLocalOrigem  string  `json:"descricaoLocalOrigem"`
	LocalDestino          int     `json:"localDestino"`
	DescricaoLocalDestino string  `json:"descricaoLocalDestino"`
	TipoTransferencia     int     `json:"tipoTransferencia"`
}
