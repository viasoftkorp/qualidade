import { OrigemNaoConformidades } from './origem-nao-conformidades';
import { StatusNaoConformidade } from './status-nao-conformidades';

export interface NaoConformidadeInput
{
  id?: string;
  codigo?: number
  origem?: OrigemNaoConformidades
  status?: StatusNaoConformidade
  idNotaFiscal?: string | null;
  idNatureza?: string
  idPessoa?: string
  idProduto?: string
  revisao?: number;
  equipe?: string
  idLote?: string
  dataFabricacaoLote?: Date;
  campoNf?: string;
  idCriador?: string;
  loteTotal?: boolean;
  loteParcial?: boolean;
  rejeitado?: boolean;
  aceitoConcessao?: boolean;
  retrabalhoPeloCliente?: boolean;
  retrabalhoNoCliente?: boolean;
  naoConformidadeEmPotencial?: boolean;
  relatoNaoConformidade?: boolean;
  melhoriaEmPotencial?: boolean;
  descricao?: string;
  numeroNotaFiscal?: string
  numeroOdf?: string
  numeroLote?: string
  idPedido?: string;
  numeroOdfFaturamento?: number;
  idProdutoFaturamento?: string;
  incompleta?:boolean;
}
