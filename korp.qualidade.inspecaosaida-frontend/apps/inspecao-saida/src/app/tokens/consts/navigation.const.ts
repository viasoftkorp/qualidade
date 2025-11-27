import { ensureTrailingSlash } from '@viasoft/http';
import { IItemMenuOptions } from '@viasoft/navigation';

export const NAVIGATION_MENU_ITEMS: IItemMenuOptions[] = [
  {
    label: 'InspecaoSaida.Navigation.Title',
    path: 'inspecao-saida',
    icon: 'box-check'
  },
  {
    label: 'InspecaoSaida.Navigation.Processamento',
    path: 'processamento',
    icon: 'tasks'
  },
  // {
  //   label: 'InspecaoSaida.Navigation.Historico',
  //   path: 'historico',
  //   icon: 'history'
  // }
  {
    label: 'InspecaoSaida.Navigation.Configuracoes',
    icon: 'cog',
    children: [
      {
        label: 'InspecaoSaida.Navigation.Geral',
        icon: 'cogs',
        path: 'configuracoes'
      },
      {
        label: 'InspecaoSaida.Navigation.AlterarRelatorio',
        icon: 'sign-out-alt',
        cta: () => {
          window.open(`${ensureTrailingSlash(window.location.origin)}relatorios/reports`, '_blank', 'noopener noreferrer');
        }
      }
    ]
  },
];
