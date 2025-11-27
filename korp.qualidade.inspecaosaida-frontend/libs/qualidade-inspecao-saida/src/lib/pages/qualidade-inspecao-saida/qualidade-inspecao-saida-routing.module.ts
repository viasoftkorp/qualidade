import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { QualidadeInspecaoSaidaComponent } from './qualidade-inspecao-saida.component';

const routes: Routes = [
  {
    path: '',
    component: QualidadeInspecaoSaidaComponent,
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
export class QualidadeInspecaoSaidaRoutingModule {}
