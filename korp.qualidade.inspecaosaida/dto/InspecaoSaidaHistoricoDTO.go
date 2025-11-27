package dto

import "time"

type InspecaoSaidaHistoricoCabecalhoDTO struct {
	OdfApontada        int     `json:"odfApontada"`
	OrdemFabricacao    int     `json:"ordemFabricacao"`
	CodigoProduto      string  `json:"codigoProduto"`
	DescricaoProduto   string  `json:"descricaoProduto"`
	QuantidadeLote     float64 `json:"quantidadeLote"`
	QuantidadeInspecao float64 `json:"quantidadeInspecao"`
	Lote               string  `json:"lote"`
	NumeroPedido       string  `json:"numeroPedido"`
	Cliente            string  `json:"cliente"`
	Plano              string  `json:"plano"`
	DescricaoPlano     string  `json:"descricaoPlano"`
	DataNegociada      string  `json:"dataNegociada"`
	Revisao            string  `json:"revisao"`
}

type GetAllInspecaoSaidaHistoricoCabecalhoDTO struct {
	Items      []InspecaoSaidaHistoricoCabecalhoDTO `json:"items"`
	TotalCount int64                                `json:"totalCount"`
}

type InspecaoSaidaHistoricoCabecalhoFilters struct {
	OrdemFabricacao *int    `json:"ordemFabricacao"`
	CodigoProduto   *string `json:"codigoProduto"`
	Lote            *string `json:"lote"`
}

type InspecaoSaidaHistoricoItems struct {
	CodigoInspecao         int        `json:"codigoInspecao"`
	RecnoInspecao          int        `json:"recnoInspecao"`
	OdfApontada            int        `json:"odfApontada"`
	OrdemFabricacao        int        `json:"ordemFabricacao"`
	CodigoProduto          string     `json:"codigoProduto"`
	DescricaoProduto       string     `json:"descricaoProduto"`
	QuantidadeInspecao     float64    `json:"quantidadeInspecao"`
	QuantidadeRetrabalhada float64    `json:"quantidadeRetrabalhada"`
	QuantidadeAprovada     float64    `json:"quantidadeAprovada"`
	QuantidadeReprovada    float64    `json:"quantidadeReprovada"`
	Inspetor               string     `json:"inspetor"`
	TipoInspecao           string     `json:"tipoInspecao"`
	Resultado              string     `json:"resultado"`
	DataInspecao           *time.Time `json:"dataInspecao,omitempty"`
	NumeroPedido           string     `json:"numeroPedido"`
	Cliente                string     `json:"cliente"`
	CodigoRnc              *int       `json:"codigoRnc"`
	IdRnc                  *string    `json:"idRnc"`
}

type InspecaoSaidaHistoricoItemsDTO struct {
	RecnoInspecao          int                                         `json:"recnoInspecao"`
	CodigoInspecao         int                                         `json:"codigoInspecao"`
	OdfApontada            int                                         `json:"odfApontada"`
	OrdemFabricacao        int                                         `json:"ordemFabricacao"`
	NumeroPedido           string                                      `json:"numeroPedido"`
	Cliente                string                                      `json:"cliente"`
	CodigoProduto          string                                      `json:"codigoProduto"`
	DescricaoProduto       string                                      `json:"descricaoProduto"`
	QuantidadeInspecao     float64                                     `json:"quantidadeInspecao"`
	QuantidadeRetrabalhada float64                                     `json:"quantidadeRetrabalhada"`
	QuantidadeAprovada     float64                                     `json:"quantidadeAprovada"`
	QuantidadeReprovada    float64                                     `json:"quantidadeReprovada"`
	Inspetor               string                                      `json:"inspetor"`
	TipoInspecao           string                                      `json:"tipoInspecao"`
	Resultado              string                                      `json:"resultado"`
	DataInspecao           *time.Time                                  `json:"dataInspecao,omitempty"`
	OdfRetrabalho          *int                                        `json:"odfRetrabalho"`
	CodigoRnc              *int                                        `json:"codigoRnc"`
	IdRnc                  *string                                     `json:"idRnc"`
	Transferencias         []HistoricoInspecaoSaidaTransferenciaOutput `json:"transferencias"`
}

type GetAllInspecaoSaidaHistoricoItemsDTO struct {
	Items      []InspecaoSaidaHistoricoItemsDTO `json:"items"`
	TotalCount int64                            `json:"totalCount"`
}

type HistoricoInspecaoSaidaTransferenciaOutput struct {
	OrdemFabricacao       int     `json:"ordemFabricacao"`
	Quantidade            float64 `json:"quantidade"`
	NumeroPedido          string  `json:"numeroPedido"`
	LocalOrigem           int     `json:"localOrigem"`
	DescricaoLocalOrigem  string  `json:"descricaoLocalOrigem"`
	LocalDestino          int     `json:"localDestino"`
	DescricaoLocalDestino string  `json:"descricaoLocalDestino"`
	TipoTransferencia     int     `json:"tipoTransferencia"`
}
