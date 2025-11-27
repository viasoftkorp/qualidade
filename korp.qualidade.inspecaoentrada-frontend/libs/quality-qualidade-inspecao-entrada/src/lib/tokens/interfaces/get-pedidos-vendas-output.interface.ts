import { PedidoVendaLoteDto } from './pedido-venda-lote-interface-dto';

export interface EstoqueLocalPedidoVendaAlocacaoDTO {
  id: string;
  numeroPedido: number;
  numeroOdf: number;
  quantidadeTotalPedido: number;
  quantidadeAlocadaLoteLocal: number;
  quantidadeEntrada: number;
  quantidadeRestanteInspecionar: number;
  descricaoProduto: string;
  descricaoLocalReprovado: string;
  descricaoLocalAprovado: string;
  codigoLocalReprovado: number;
  codigoLocalAprovado: number;
  quantidadeAprovada: number;
  quantidadeReprovada: number;
  lotes: Array<PedidoVendaLoteDto>
}

export interface GetAllEstoqueLocalPedidoVendaAlocacaoDTO {
  items: EstoqueLocalPedidoVendaAlocacaoDTO[]
  totalCount: number
}
