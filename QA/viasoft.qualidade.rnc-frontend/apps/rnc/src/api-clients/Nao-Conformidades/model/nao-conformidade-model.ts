import { OrigemNaoConformidades } from './origem-nao-conformidades';
import { StatusNaoConformidade } from './status-nao-conformidades';

export class NaoConformidadeModel {
  public id: string;
  public codigo :number
  public origem: OrigemNaoConformidades
  public status:StatusNaoConformidade
  public idNotaFiscal:string;
  public idNatureza:string
  public descricaoNatureza:string
  public codigoNatureza:number
  public natureza:string
  public idPessoa:string
  public idCliente:string
  public nomeCliente:string
  public codigoCliente:number
  public cliente:string
  public idFornecedor:string
  public nomeFornecedor:string
  public codigoFornecedor:number
  public fornecedor:string
  public numeroOdf:number
  public idProduto:string
  public descricaoProduto:string
  public codigoProduto:number
  public produto:string
  public revisao:number
  public equipe:string
  public idLote : string
  public numeroLote : string
  public dataFabricacaoLote: Date
  public campoNf: string
  public idCriador: string
  public usuario:string;
  public loteTotal:boolean
  public loteParcial:boolean
  public rejeitado:boolean
  public aceitoConcessao: boolean
  public retrabalhoPeloCliente: boolean
  public retrabalhoNoCliente:boolean
  public naoConformidadeEmPotencial: boolean
  public relatoNaoConformidade: boolean
  public melhoriaEmPotencial: boolean
  public descricao: string
  public numeroOdfFaturamento: string
  public idProdutoFaturamento: string
  public numeroPedido: string
  public dataCriacao: Date
  public companyId: string
}
