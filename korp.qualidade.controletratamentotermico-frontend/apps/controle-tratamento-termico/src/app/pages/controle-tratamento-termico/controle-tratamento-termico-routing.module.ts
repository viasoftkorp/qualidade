import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ControleTratamentoTermicoComponent } from './controle-tratamento-termico.component';

const routes: Routes = [
  {
    path: '',
    component: ControleTratamentoTermicoComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ControleTratamentoTermicoRoutingModule {}
