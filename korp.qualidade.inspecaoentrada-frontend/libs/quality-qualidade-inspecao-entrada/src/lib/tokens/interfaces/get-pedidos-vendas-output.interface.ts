export interface EstoqueLocalPedidoVendaAlocacaoDTO {
  id: string;
  numeroPedido: number;
  quantidadeTotalPedido: number;
  quantidadeAlocadaLoteLocal: number;
  descricaoProduto: string;
  descricaoLocalReprovado: string;
  descricaoLocalAprovado: string;
  codigoLocalReprovado: number;
  codigoLocalAprovado: number;
  quantidadeAprovada: number;
  quantidadeReprovada: number;
}

export interface GetAllEstoqueLocalPedidoVendaAlocacaoDTO {
  items: EstoqueLocalPedidoVendaAlocacaoDTO[]
  totalCount: number
}
