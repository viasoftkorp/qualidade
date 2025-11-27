import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'processamento',
    loadChildren: () => import('@viasoft/qualidade-inspecao-saida').then((m) => m.ProcessamentoInspecaoModule),
  },
  {
    path: 'historico',
    loadChildren: () => import('@viasoft/qualidade-inspecao-saida').then((m) => m.HistoricoInspecaoModule),
  },
  {
    path: '',
    loadChildren: () => import('@viasoft/qualidade-inspecao-saida').then((m) => m.QualidadeInspecaoSaidaModule),
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
export class PagesRoutingModule {}
