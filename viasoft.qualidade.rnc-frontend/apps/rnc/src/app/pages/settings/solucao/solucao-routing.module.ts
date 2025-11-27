import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { SolucaoComponent } from './solucao.component';

const routes: Routes = [
  {
    path: '',
    component: SolucaoComponent
  },
  {
    path: ':id',
    loadChildren: () => import('./solucao-editor/solucao-editor.module').then((m) => m.SolucaoEditorModule)
  },
  {
    path: 'new',
    loadChildren: () => import('./solucao-editor/solucao-editor.module').then((m) => m.SolucaoEditorModule)
  },
  {
    path: '**',
    redirectTo: '',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SolucaoRoutingModule { }
