import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'home',
    loadChildren: () => import('@viasoft/quality-qualidade-inspecao-entrada').then((m) => m.QualidadeInspecaoEntradaModule),
  },
  {
    path: 'processamento',
    loadChildren: () => import('@viasoft/quality-qualidade-inspecao-entrada').then((m) => m.ProcessamentoInspecaoModule),
  },
  {
    path: 'historico',
    loadChildren: () => import('@viasoft/quality-qualidade-inspecao-entrada').then((m) => m.HistoricoModule),
  },
  {
    path: 'configuracoes-plano-amostragem',
    loadChildren: () => import('@viasoft/quality-qualidade-inspecao-entrada').then((m) => m.ConfiguracoesModule),
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule {}