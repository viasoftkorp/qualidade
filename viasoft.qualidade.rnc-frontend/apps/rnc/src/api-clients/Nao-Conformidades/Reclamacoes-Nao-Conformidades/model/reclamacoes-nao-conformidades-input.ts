export class ReclamacoesNaoConformidadesInput {
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

  constructor(idNaoConformidade: string, reclamacao:ReclamacoesNaoConformidadesInput) {
    this.id = reclamacao.id;
    this.idNaoConformidade = idNaoConformidade;
    this.retrabalhoComOnus = reclamacao.retrabalhoComOnus;
    this.retrabalhoSemOnus = reclamacao.retrabalhoSemOnus;
    this.devolucaoFornecedor = reclamacao.devolucaoFornecedor;
    this.recodificar = reclamacao.recodificar;
    this.sucata = reclamacao.sucata;
    this.observacao = reclamacao.observacao;
    this.procedentes = Number(reclamacao.procedentes);
    this.improcedentes = Number(reclamacao.improcedentes);
    this.quantidadeLote = Number(reclamacao.quantidadeLote);
    this.quantidadeNaoConformidade = Number(reclamacao.quantidadeNaoConformidade);
    this.disposicaoProdutosAprovados = Number(reclamacao.disposicaoProdutosAprovados);
    this.disposicaoProdutosConcessao = Number(reclamacao.disposicaoProdutosConcessao);
    this.rejeitado = Number(reclamacao.rejeitado);
    this.retrabalho = Number(reclamacao.retrabalho);
  }
}
