export interface PlanoInspecaoDTO {
  id: string;
  descricao: string;
  resultado: string;
  maiorValor: number;
  menorValor: number;
  maiorValorBase: number;
  menorValorBase: number;
  metodo: string;
  observacao: string;
}

export interface GetPlanosInspecaoDTO {
  items: PlanoInspecaoDTO[];
  totalCount: number;
}
