import { NotaFiscalDTO } from './get-notas-fiscais-dto.interface';

export interface InspecaoDetailsDTO {
  notaFiscal: NotaFiscalDTO;
  codigoInspecao: number;
  novaInspecao: boolean;
  codigoProduto: string;
  codigoFornecedor: string;
}
