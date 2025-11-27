import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InspecaoHistoricoViewComponent } from './inspecao-historico-view/inspecao-historico-view.component';

const routes: Routes = [
  {
    path: '',
    component: InspecaoHistoricoViewComponent,
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
export class HistoricoRoutingModule { }
