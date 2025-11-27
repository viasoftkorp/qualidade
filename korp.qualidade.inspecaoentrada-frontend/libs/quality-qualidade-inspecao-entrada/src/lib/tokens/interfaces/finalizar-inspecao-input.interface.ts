import { PedidoVendaLoteDto } from './pedido-venda-lote-interface-dto';

export interface FinalizarInspecaoInput {
  codigoInspecao: number;
  quantidadeAprovada: number;
  quantidadeRejeitada: number;
  codigoLocalPrincipal: number;
  codigoLocalReprovado: number;
  idRnc?: string;
  lotes: Array<PedidoVendaLoteDto>
}
