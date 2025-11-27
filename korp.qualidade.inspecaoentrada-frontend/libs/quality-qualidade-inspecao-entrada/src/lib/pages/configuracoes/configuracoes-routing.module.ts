import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlanoAmostragemComponent } from './plano-amostragem/plano-amostragem.component';

const routes: Routes = [
  {
    path: '',
    component: PlanoAmostragemComponent,
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
