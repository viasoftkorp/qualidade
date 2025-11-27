export interface NotaFiscalDTO {
  notaFiscal: number;
  lote: string;
  plano: number;
  codigoProduto: string;
  descricaoProduto: string;
  descricaoForneced: string;
  quantidade: number;
  quantidadeInspecionada: number;
  quantidadeInspecionar: number;
  dataEntrada: Date;
}

export interface GetNotasFiscaisDTO {
  items: NotaFiscalDTO[];
  totalCount: number;
}
