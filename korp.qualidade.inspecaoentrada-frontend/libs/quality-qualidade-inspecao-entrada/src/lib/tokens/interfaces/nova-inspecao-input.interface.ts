import { PlanoInspecaoDTO } from './get-planos-inspecao-dto.interface';

export interface NovaInspecaoInput {
  recnoItemNotaFiscal: number;
  notaFiscal: number;
  lote: string;
  codigoProduto: string;
  quantidade: number;
  plano: string;
  planosInspecao: PlanoInspecaoDTO[];
  serieNotaFiscal: string;
}
