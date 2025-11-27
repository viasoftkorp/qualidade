export interface FinalizarInspecaoInput {
  codInspecao: number;
  quantidadeAprovada: number;
  quantidadeReprovada: number;
  quantidadeRetrabalhada: number;
  codigoLocalAprovado: number;
  codigoLocalReprovado: number;
  codigoLocalRetrabalho: number;
  rnc?: {
    idRnc?: string;
    codigoRnc?: number;
    materiais: Array<{
      quantidade: number;
      idProduto: string;
      operacaoEngenharia: string;
    }>;
    recursos: Array<{
      horas: number;
      detalhamento: string;
      idRecurso: string;
      operacaoEngenharia: string;
    }>;
  }
}
