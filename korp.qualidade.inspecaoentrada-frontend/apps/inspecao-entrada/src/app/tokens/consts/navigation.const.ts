import { ensureTrailingSlash } from '@viasoft/http';
import { IItemMenuOptions } from '@viasoft/navigation';

export const NAVIGATION_MENU_ITEMS: IItemMenuOptions[] = [
  {
    label: 'InspecaoEntrada.Navigation.Title',
    path: 'home',
    icon: 'box-check',
    exactMatch: false,
  },
  {
    label: 'InspecaoEntrada.Navigation.Processamento',
    path: 'processamento',
    icon: 'tasks',
    exactMatch: false,
  },
  {
    label: 'InspecaoEntrada.Navigation.Configuracoes',
    icon: 'cog',
    children: [
      {
        label: 'InspecaoEntrada.Navigation.PlanoAmostragem',
        path: '/configuracoes-plano-amostragem',
        icon: 'ballot',
      },
      {
        label: 'InspecaoEntrada.Navigation.AlterarRelatorio',
        icon: 'sign-out-alt',
        cta: () => {
          window.open(`${ensureTrailingSlash(window.location.origin)}relatorios/reports`, '_blank', 'noopener noreferrer');
        }
      }
    ]
  }
];
