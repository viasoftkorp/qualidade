import { NaoConformidadeOutput } from './nao-conformidade-output';
import { AcoesPreventivasNaoConformidadesModel } from '../Acoes-Preventivas-Nao-Conformidades/model'
import { CausasNaoConformidadesModel } from '../Causas-Nao-Conformidades/model'
import { ConclusoesNaoConformidadesOutput } from '../Conclusoes-Nao-Conformidades/model'
import { DefeitosNaoConformidadesModel } from '../Defeitos-Nao-Conformidades/model'
import { ReclamacoesNaoConformidadesOutput } from '../Reclamacoes-Nao-Conformidades/model'
import { ServicosNaoConformidadesOutput } from '../Servicos-Solucoes-Nao-Conformidades/model'
import { SolucoesNaoConformidadesModel } from '../Solucoes-Nao-Conformidades/model'
import { ProdutosNaoConformidadesOutput } from '../Produtos-Nao-Conformidades/model';

export interface NaoConformidadeAgregacaoOutput {
  naoConformidade: NaoConformidadeOutput;
  conclusaoNaoConformidade: ConclusoesNaoConformidadesOutput;
  reclamacaoNaoConformidade: ReclamacoesNaoConformidadesOutput;
  acaoPreventivaNaoConformidades: AcoesPreventivasNaoConformidadesModel[];
  causaNaoConformidades: CausasNaoConformidadesModel[];
  defeitoNaoConformidades: DefeitosNaoConformidadesModel[];
  solucaoNaoConformidades: SolucoesNaoConformidadesModel[];
  produtoNaoConformidades: ProdutosNaoConformidadesOutput[];
  servicoNaoConformidades: ServicosNaoConformidadesOutput[];
}
