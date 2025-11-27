package dto

type EstoqueLocalPedidoVendaAlocacaoDTO struct {
	IdPedidoVenda                 string  `json:"id"`
	NumeroPedido                  string  `json:"numeroPedido"`
	NumeroOdf                     int     `json:"numeroOdf"`
	QuantidadeTotalPedido         float64 `json:"quantidadeTotalPedido"`
	QuantidadeAlocadaLoteLocal    float64 `json:"quantidadeAlocadaLoteLocal"`
	QuantidadeEntrada             float64 `json:"quantidadeEntrada"`
	QuantidadeRestanteInspecionar float64 `json:"quantidadeRestanteInspecionar"`
	QuantidadeLote                float64 `json:"quantidadeLote"`
	DescricaoProduto              string  `json:"descricaoProduto"`
	DescricaoLocalReprovado       string  `json:"descricaoLocalReprovado"`
	DescricaoLocalAprovado        string  `json:"descricaoLocalAprovado"`
	CodigoLocalReprovado          int     `json:"codigoLocalReprovado"`
	CodigoLocalAprovado           int     `json:"codigoLocalAprovado"`
	QuantidadeAprovada            float64 `json:"quantidadeAprovada"`
	QuantidadeReprovada           float64 `json:"quantidadeReprovada"`
}

type GetAllEstoqueLocalPedidoVendaAlocacaoDTO struct {
	Items      []EstoqueLocalPedidoVendaAlocacaoDTO `json:"items"`
	TotalCount int64                                `json:"totalCount"`
}
