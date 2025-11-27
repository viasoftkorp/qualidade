import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { SolucaoEditorComponent } from './solucao-editor.component';

const routes: Routes = [
  {
    path: '',
    component: SolucaoEditorComponent,
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
export class SolucaoEditorRoutingModule { }
