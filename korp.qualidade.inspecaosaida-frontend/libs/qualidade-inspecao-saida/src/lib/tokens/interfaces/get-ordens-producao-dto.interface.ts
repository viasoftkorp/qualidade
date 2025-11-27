export interface OrdemProducaoDTO {
  odf: number;
  odfApontada: number;
  lote: string;
  codigoProduto: string;
  descricaoProduto: string;
  situacao: string;
  revisao: string;
  quantidadeOrdem: number;
  quantidadeProduzida: number;
  saldo: number;
  dataInicio: Date;
  dataEntrega: Date;
  dataEmissao: Date;
  quantidadeInspecionada: number;
  quantidadeInspecionar: number;
  plano: string;
  recnoProcesso: number;
}

export interface GetOrdensProducaoDTO {
  items: OrdemProducaoDTO[];
  totalCount: number;
}
