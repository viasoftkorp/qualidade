import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { NaoConformidadesEditorComponent } from './nao-conformidades-editor.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: NaoConformidadesEditorComponent,
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
export class NaoConformidadesEditorRoutingModule { }
