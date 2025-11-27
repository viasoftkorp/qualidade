import { PlanoInspecaoDTO } from './get-planos-inspecao-dto.interface';

export interface NovaInspecaoInput {
  notaFiscal: number;
  lote: string;
  codigoProduto: string;
  quantidade: number;
  plano: number;
  planosInspecao: PlanoInspecaoDTO[];
}
