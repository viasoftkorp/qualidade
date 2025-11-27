import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { CausaComponent } from './causa.component';

const routes: Routes = [
  {
    path: '',
    component: CausaComponent,
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
export class CausaRoutingModule { }
