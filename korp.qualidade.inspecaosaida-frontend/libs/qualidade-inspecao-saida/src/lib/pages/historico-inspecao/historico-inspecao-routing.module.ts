import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HistoricoInspecaoComponent } from './historico-inspecao.component';

const routes: Routes = [
  {
    path: '',
    component: HistoricoInspecaoComponent,
  },
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HistoricoInspecaoRoutingModule { }
