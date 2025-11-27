import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {ConfiguracoesComponent} from "./configuracoes.component";

const routes: Routes = [
  {
    path: '',
    component: ConfiguracoesComponent,
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
export class ConfiguracoesRoutingModule { }
