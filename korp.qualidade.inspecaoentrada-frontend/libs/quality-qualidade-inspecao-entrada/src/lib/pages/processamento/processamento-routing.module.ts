import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ProcessamentoInspecaoComponent } from './processamento.component';

const routes: Routes = [
  {
    path: '',
    component: ProcessamentoInspecaoComponent,
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
export class ProcessamentoInspecaoRoutingModule { }
