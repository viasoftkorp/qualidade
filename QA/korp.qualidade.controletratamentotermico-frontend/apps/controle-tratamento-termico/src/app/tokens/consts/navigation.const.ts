import { IItemMenuOptions } from '@viasoft/navigation';
import { PreCadastrosPolicies } from '../../pages/configuracoes/pre-cadastros/pre-cadastros.component';

export const NAVIGATION: IItemMenuOptions[] = [
  {
    icon: 'temperature-high',
    label: 'ControleTratamentoTermico.Navigation.Title',
    path: 'controle-tratamento-termico',
  },
  {
    icon: 'cog',
    label: 'ControleTratamentoTermico.Navigation.Configuracoes',
    authorizations: [...PreCadastrosPolicies],
    children: [
      {
        icon: 'cabinet-filing',
        label: 'ControleTratamentoTermico.Navigation.PreCadastros.Title',
        authorizations: PreCadastrosPolicies,
        authorizationOperator: 'OR',
        authorizationType: 'HIDE',
        path: 'configuracoes/pre-cadastros',
      }
    ],
  },
];
