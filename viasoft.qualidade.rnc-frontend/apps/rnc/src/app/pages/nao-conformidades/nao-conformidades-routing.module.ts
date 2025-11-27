import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NaoConformidadeComponent } from './nao-conformidades.component';

const routes: Routes = [
  {
    path: '',
    component: NaoConformidadeComponent,
  },
  {
    path: ':id',
    loadChildren: () => import('./nao-conformidades-editor/nao-conformidades-editor.module')
      .then((m) => m.NaoConformidadesEditorModule)
  },
  {
    path: 'new',
    loadChildren: () => import('./nao-conformidades-editor/nao-conformidades-editor.module')
      .then((m) => m.NaoConformidadesEditorModule)
  },
  {
    path: '**', redirectTo: 'nao-conformidades'
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NaoConformidadesRoutingModule {}
