export class SolucaoServicoModel {
  public id: string;
  public idSolucao: string;
  public idProduto: string;
  public codigo: string;
  public descricao: string;
  public quantidade: number;
  public horas?: number;
  public minutos?: number;
  public idRecurso?: string;
  public codigoRecurso: string;
  public descricaoRecurso: string;
  public operacaoEngenharia?: string;
}
