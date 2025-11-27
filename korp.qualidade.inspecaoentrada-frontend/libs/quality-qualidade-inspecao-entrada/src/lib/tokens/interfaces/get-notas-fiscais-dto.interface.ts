export interface NotaFiscalDTO {
  id: string;
  recno: number;
  notaFiscal: number;
  lote: string;
  plano: string;
  codigoProduto: string;
  descricaoProduto: string;
  codigoForneced: string;
  descricaoForneced: string;
  quantidade: number;
  quantidadeInspecionada: number;
  quantidadeInspecionar: number;
  dataEntrada: Date;
  serie: string;
  observacao: string;
}

export interface GetNotasFiscaisDTO {
  items: NotaFiscalDTO[];
  totalCount: number;
}
