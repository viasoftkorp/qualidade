export class ReclamacoesNaoConformidadesOutput {
  public id: string
  public idNaoConformidade: string
  public procedentes: number
  public improcedentes: number
  public quantidadeLote: number
  public quantidadeNaoConformidade: number
  public disposicaoProdutosAprovados: number
  public disposicaoProdutosConcessao: number
  public rejeitado: number
  public retrabalho: number
  public retrabalhoComOnus: boolean
  public retrabalhoSemOnus: boolean
  public devolucaoFornecedor: boolean
  public recodificar: boolean
  public sucata: boolean
  public observacao: string
}
