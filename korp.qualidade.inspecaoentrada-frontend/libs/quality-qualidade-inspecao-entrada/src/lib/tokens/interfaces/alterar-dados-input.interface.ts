import { InspecaoEntradaItemDTO } from './get-inspecao-entrada-itens-dto.interface';
import { EstoqueLocalPedidoVendaAlocacaoDTO } from './get-pedidos-vendas-output.interface';
import { PlanoInspecaoDTO } from './get-planos-inspecao-dto.interface';
import { InspecaoDetailsDTO } from './inspecao-details-dto.class';

export interface AlterarDadosDTO {
  dto: PlanoInspecaoDTO | InspecaoEntradaItemDTO,
  inspecaoDetais: InspecaoDetailsDTO
}

export interface AlterarDadosPedidoDTO {
  pedidoVenda: EstoqueLocalPedidoVendaAlocacaoDTO;
  recnoInspecaoEntrada: number;
  quantidadeLote: number;
  quantidadeInspecao: number;
  codigoProduto: string;
}
