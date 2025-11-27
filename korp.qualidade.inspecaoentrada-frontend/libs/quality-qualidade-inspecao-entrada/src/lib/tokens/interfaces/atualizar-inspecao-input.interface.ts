import { InspecaoEntradaItemDTO } from './get-inspecao-entrada-itens-dto.interface';

export interface AtualizarInspecaoInput {
  codigoInspecao: number;
  itens: InspecaoEntradaItemDTO[];
  quantidadeInspecao: number;
}
