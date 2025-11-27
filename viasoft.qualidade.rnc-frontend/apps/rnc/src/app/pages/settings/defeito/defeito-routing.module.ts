import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { DefeitoComponent } from './defeito.component';

const routes: Routes = [
  {
    path: '',
    component: DefeitoComponent,
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
export class DefeitoRoutingModule { }
