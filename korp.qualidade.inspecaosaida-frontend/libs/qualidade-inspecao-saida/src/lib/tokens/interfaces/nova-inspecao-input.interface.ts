import { PlanoInspecaoDTO } from './get-planos-inspecao-dto.interface';

export interface NovaInspecaoInput {
  odf: number;
  codProduto: string;
  plano: string;
  quantidade: number;
  lote: string;
  planosInspecao: PlanoInspecaoDTO[];
}
