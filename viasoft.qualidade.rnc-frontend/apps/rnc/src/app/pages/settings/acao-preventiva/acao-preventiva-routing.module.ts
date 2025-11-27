import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { AcaoPreventivaComponent } from './acao-preventiva.component';

const routes: Routes = [
  {
    path: '',
    component: AcaoPreventivaComponent,
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
export class AcaoPreventivaRoutingModule { }
