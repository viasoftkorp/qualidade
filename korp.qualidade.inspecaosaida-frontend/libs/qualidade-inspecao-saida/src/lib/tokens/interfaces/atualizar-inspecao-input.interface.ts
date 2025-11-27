import { InspecaoSaidaItemDTO } from './get-inspecao-saida-itens-dto.interface';

export interface AtualizarInspecaoInput {
  codInspecao: number;
  itens: InspecaoSaidaItemDTO[];
  quantidadeInspecao: number;
  lote: string;
}
