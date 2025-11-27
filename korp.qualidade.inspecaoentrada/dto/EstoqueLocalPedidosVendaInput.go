package dto

type EstoqueLocalPedidoVendaAlocacaoInput struct {
	IdPedidoVenda              string                `json:"id"`
	NumeroPedido               string                `json:"numeroPedido"`
	QuantidadeTotalPedido      float64               `json:"quantidadeTotalPedido"`
	QuantidadeAlocadaLoteLocal float64               `json:"quantidadeAlocadaLoteLocal"`
	QuantidadeLote             float64               `json:"quantidadeLote"`
	DescricaoProduto           string                `json:"descricaoProduto"`
	DescricaoLocalReprovado    string                `json:"descricaoLocalReprovado"`
	DescricaoLocalAprovado     string                `json:"descricaoLocalAprovado"`
	CodigoLocalReprovado       int                   `json:"codigoLocalReprovado"`
	CodigoLocalAprovado        int                   `json:"codigoLocalAprovado"`
	QuantidadeAprovada         float64               `json:"quantidadeAprovada"`
	QuantidadeReprovada        float64               `json:"quantidadeReprovada"`
	Lotes                      []*PedidoVendaLoteDto `json:"lotes"`
}
