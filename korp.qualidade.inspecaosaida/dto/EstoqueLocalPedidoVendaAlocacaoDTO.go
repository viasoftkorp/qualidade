package dto

type EstoqueLocalPedidoVendaAlocacaoDTO struct {
	Id                         string  `json:"id"`
	NumeroPedido               string  `json:"numeroPedido"`
	OrdemFabricacao            int     `json:"ordemFabricacao"`
	QuantidadeTotalPedido      float64 `json:"quantidadeTotalPedido"`
	QuantidadeAlocadaLoteLocal float64 `json:"quantidadeAlocadaLoteLocal"`
	DescricaoProduto           string  `json:"descricaoProduto"`
	DescricaoLocalReprovado    string  `json:"descricaoLocalReprovado"`
	DescricaoLocalRetrabalho   string  `json:"descricaoLocalRetrabalho"`
	DescricaoLocalAprovado     string  `json:"descricaoLocalAprovado"`
	CodigoLocalReprovado       int     `json:"codigoLocalReprovado"`
	CodigoLocalRetrabalho      int     `json:"codigoLocalRetrabalho"`
	CodigoLocalAprovado        int     `json:"codigoLocalAprovado"`
	QuantidadeAprovada         float64 `json:"quantidadeAprovada"`
	QuantidadeReprovada        float64 `json:"quantidadeReprovada"`
	QuantidadeRetrabalhada     float64 `json:"quantidadeRetrabalhada"`
}

type GetAllEstoqueLocalPedidoVendaAlocacaoDTO struct {
	Items      []EstoqueLocalPedidoVendaAlocacaoDTO `json:"items"`
	TotalCount int64                                `json:"totalCount"`
}
