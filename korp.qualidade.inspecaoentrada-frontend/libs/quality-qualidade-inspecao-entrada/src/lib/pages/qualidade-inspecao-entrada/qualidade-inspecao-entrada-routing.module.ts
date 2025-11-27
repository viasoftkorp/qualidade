import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { QualidadeInspecaoEntradaComponent } from './qualidade-inspecao-entrada.component';

const routes: Routes = [
  {
    path: '',
    component: QualidadeInspecaoEntradaComponent,
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
export class QualidadeInspecaoEntradaRoutingModule {}
