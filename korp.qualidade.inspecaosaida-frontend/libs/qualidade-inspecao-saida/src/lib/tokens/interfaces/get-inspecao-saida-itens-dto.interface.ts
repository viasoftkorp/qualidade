export interface InspecaoSaidaItemDTO {
  id: string;
  plano: string;
  odf: number;
  descricao: string;
  metodo: string;
  sequencia: string;
  resultado: string;
  maiorValor: number;
  menorValor: number;
  maiorValorBase: number;
  menorValorBase: number;
  observacao: string;
}

export interface GetInspecaoSaidaItensDTO {
  items: InspecaoSaidaItemDTO[];
  totalCount: number;
}
