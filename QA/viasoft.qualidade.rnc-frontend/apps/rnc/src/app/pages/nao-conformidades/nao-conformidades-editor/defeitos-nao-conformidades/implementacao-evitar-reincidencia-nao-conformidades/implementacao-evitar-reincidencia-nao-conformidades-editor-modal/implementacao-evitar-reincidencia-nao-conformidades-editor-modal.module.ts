import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsButtonModule, VsCheckboxModule, VsDatepickerModule, VsDialogModule, VsFormModule, VsLabelModule, VsLayoutModule, VsSpinnerModule, VsTextareaModule } from '@viasoft/components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// eslint-disable-next-line max-len
import { ImplementacaoEvitarReincidenciaNaoConformidadesEditorModalComponent } from './implementacao-evitar-reincidencia-nao-conformidades-editor-modal.component';
import { UserModule } from '@viasoft/common';
import { UserAutocompleteModule } from '@viasoft/administration';

@NgModule({
  declarations: [
    ImplementacaoEvitarReincidenciaNaoConformidadesEditorModalComponent
  ],
  imports: [
    CommonModule,
    VsDialogModule,
    VsLabelModule,
    VsFormModule,
    ReactiveFormsModule,
    FormsModule,
    VsDatepickerModule,
    VsButtonModule,
    UserModule,
    UserAutocompleteModule,
    VsTextareaModule,
    VsCheckboxModule,
    VsLayoutModule,
    VsSpinnerModule
  ]
})
export class ImplementacaoEvitarReincidenciaNaoConformidadesEditorModalModule { }
