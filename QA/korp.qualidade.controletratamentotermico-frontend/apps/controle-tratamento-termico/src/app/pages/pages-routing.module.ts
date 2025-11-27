import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VsAuthorizationGuard } from '@viasoft/common';
import { LastPageAcessedActivate } from '../shared/last-page-acessed-activate';

const routes: Routes = [
  {
    path: 'configuracoes',
    loadChildren: () => import('./configuracoes/configuracoes.module').then((m) => m.ConfiguracoesModule),
    canActivate: [VsAuthorizationGuard, LastPageAcessedActivate]
  },
  {
    path: '',
    loadChildren: () => import('./controle-tratamento-termico/controle-tratamento-termico.module').then((m) => m.ControleTratamentoTermicoModule),
    canActivate: [LastPageAcessedActivate]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
