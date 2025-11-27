import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HistoricoInspecaoViewComponent } from '@viasoft/qualidade-inspecao-saida';

const routes: Routes = [
  {
    path: '',
    component: HistoricoInspecaoViewComponent,
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
