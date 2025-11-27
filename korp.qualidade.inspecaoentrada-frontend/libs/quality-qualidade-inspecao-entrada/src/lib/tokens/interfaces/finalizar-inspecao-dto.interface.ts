import { InspecaoEntradaDTO } from './get-inspecao-entrada-dto.interface';
import { NotaFiscalDTO } from './get-notas-fiscais-dto.interface';

export interface FinalizarInspecaoModalData {
  notaFiscal: NotaFiscalDTO;
  inspecaoEntrada: InspecaoEntradaDTO;
  codigoProduto: string;
}
