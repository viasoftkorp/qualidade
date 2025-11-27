import { authorizationNavigationConfig } from '@viasoft/authorization-management';
import { IItemMenuOptions } from '@viasoft/navigation';
import { Policies } from '../../services/authorizations/policies/policies';
import { NaoConformidadePolicies } from '../../services/authorizations/policies/nao-conformidade-policies';

const CONFIGURACAO_POLICIES = [
  Policies.CreateAcaoPreventiva,
  Policies.ReadAcaoPreventiva,
  Policies.DeleteAcaoPreventiva,
  Policies.UpdateAcaoPreventiva,

  Policies.CreateDefeito,
  Policies.ReadDefeito,
  Policies.DeleteDefeito,
  Policies.UpdateDefeito,

  Policies.CreateSolucao,
  Policies.ReadSolucao,
  Policies.DeleteSolucao,
  Policies.UpdateSolucao,

  Policies.CreateNatureza,
  Policies.ReadNatureza,
  Policies.DeleteNatureza,
  Policies.UpdateNatureza,

  Policies.CreateCausa,
  Policies.ReadCausa,
  Policies.DeleteCausa,
  Policies.UpdateCausa,

  Policies.AtualizarConfiguracoesGerais,
];

export const NAVIGATION_MENU_ITEMS: IItemMenuOptions[] = [
  {
    icon: 'fragile',
    label: 'Rnc.Navigation.NaoConformidades',
    path: 'nao-conformidades',
    authorizations: NaoConformidadePolicies.ReadNaoConformidade
  },
  authorizationNavigationConfig,
  {
    label: 'Rnc.Navigation.Configuracoes',
    path: 'configuracoes',
    icon: 'cog',
    children: [
      {
        icon: 'leaf',
        label: 'Rnc.Navigation.Natureza',
        path: 'configuracoes/naturezas',
        authorizations: Policies.ReadNatureza
      },
      {
        icon: 'comment-alt',
        label: 'Rnc.Navigation.Causa',
        path: 'configuracoes/causas',
        authorizations: Policies.ReadCausa
      },
      {
        icon: 'lightbulb-on',
        label: 'Rnc.Navigation.Solucao',
        path: 'configuracoes/solucoes',
        authorizations: Policies.ReadSolucao
      },
      {
        icon: 'times-circle',
        label: 'Rnc.Navigation.Defeito',
        path: 'configuracoes/defeitos',
        authorizations: Policies.ReadDefeito
      },
      {
        icon: 'shield-alt',
        label: 'Rnc.Navigation.AcaoPreventiva',
        path: 'configuracoes/acoes-preventivas',
        authorizations: Policies.ReadAcaoPreventiva
      },
      {
        icon: 'cog',
        label: 'Rnc.Navigation.Geral',
        path: 'configuracoes/gerais',
        authorizations: Policies.AtualizarConfiguracoesGerais
      }
    ],
    authorizations: CONFIGURACAO_POLICIES,
    authorizationOperator: 'OR'
  }
];
