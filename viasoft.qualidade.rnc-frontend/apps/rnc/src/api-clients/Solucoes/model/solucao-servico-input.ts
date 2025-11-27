export interface SolucaoServicoInput {
  id: string;
  idSolucao: string;
  idProduto?: string;
  quantidade: number;
  horas: number;
  minutos: number;
  idRecurso: string;
  operacaoEngenharia: string;
  controlarApontamento: boolean;
}
