import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VsAuthorizationGuard } from '@viasoft/common';
import { PreCadastrosComponent, PreCadastrosPolicies } from './pre-cadastros.component';
import { LastPageAcessedActivate } from '@viasoft/controle-tratamento-termico/app/shared/last-page-acessed-activate';

const routes: Routes = [
  {
    path: '',
    component: PreCadastrosComponent,
    canActivate: [VsAuthorizationGuard, LastPageAcessedActivate],
    data: {
      authBackRoute: 'home',
      permissionOperator: "OR",
      permission: PreCadastrosPolicies,
    }
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PreCadastrosRoutingModule { }
