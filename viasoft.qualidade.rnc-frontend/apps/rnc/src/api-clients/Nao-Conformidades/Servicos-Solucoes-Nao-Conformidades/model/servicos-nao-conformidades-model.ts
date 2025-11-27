export class ServicosNaoConformidadesModel {
  public idNaoConformidade: string
  public id: string
  public idProduto: string
  public codigo: string
  public descricao: string
  public idRecurso?: string
  public descricaoRecurso: string
  public operacaoEngenharia: string
  public horas?: number
  public minutos?: number
  public quantidade: number
  public detalhamento: string
}
