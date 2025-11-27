import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { NaturezaComponent } from './natureza.component';

const routes: Routes = [
  {
    path: '',
    component: NaturezaComponent,
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
export class NaturezaRoutingModule { }
