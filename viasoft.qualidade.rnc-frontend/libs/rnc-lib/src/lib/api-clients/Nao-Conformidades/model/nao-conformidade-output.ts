import { OrigemNaoConformidades } from './origem-nao-conformidades';
import { StatusNaoConformidade } from './status-nao-conformidades';

export interface NaoConformidadeOutput {
  id: string;
  codigo: number;
  origem: OrigemNaoConformidades;
  status: StatusNaoConformidade;
  idNotaFiscal: string;
  idNatureza: string;
  idPessoa: string;
  idProduto: string;
  idLote: string;
  dataFabricacaoLote: Date;
  campoNf: string;
  idCriador: string;
  revisao: number;
  loteTotal: boolean;
  loteParcial: boolean;
  rejeitado: boolean;
  aceitoConcessao: boolean;
  retrabalhoPeloCliente: boolean;
  retrabalhoNoCliente: boolean;
  equipe: string;
  naoConformidadeEmPotencial: boolean;
  relatoNaoConformidade: boolean;
  melhoriaEmPotencial: boolean;
  descricao: string;
  numeroNotaFiscal: string;
  numeroOdf: string;
  numeroLote: string;
}
