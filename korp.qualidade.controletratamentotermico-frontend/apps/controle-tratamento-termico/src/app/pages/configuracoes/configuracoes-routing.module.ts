import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PreCadastrosPolicies } from './pre-cadastros/pre-cadastros.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'pre-cadastros',
        loadChildren: () => import('./pre-cadastros/pre-cadastros.module').then((m) => m.PreCadastrosModule),
        data: {
          authBackRoute: '',
          permission: PreCadastrosPolicies
        }
      },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class ConfiguracoesRoutingModule { }
