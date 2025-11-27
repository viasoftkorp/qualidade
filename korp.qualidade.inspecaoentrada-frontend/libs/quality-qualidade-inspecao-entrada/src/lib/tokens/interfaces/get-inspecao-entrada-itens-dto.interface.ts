export interface InspecaoEntradaItemDTO {
  id: string;
  codigoProduto: string;
  descricao: string;
  metodo: string;
  sequencia: string;
  resultado: string;
  maiorValorInspecionado: number;
  menorValorInspecionado: number;
  maiorValorBase: number;
  menorValorBase: number;
  observacao: string;
}

export interface GetInspecaoEntradaItensDTO {
  items: InspecaoEntradaItemDTO[];
  totalCount: number;
}
