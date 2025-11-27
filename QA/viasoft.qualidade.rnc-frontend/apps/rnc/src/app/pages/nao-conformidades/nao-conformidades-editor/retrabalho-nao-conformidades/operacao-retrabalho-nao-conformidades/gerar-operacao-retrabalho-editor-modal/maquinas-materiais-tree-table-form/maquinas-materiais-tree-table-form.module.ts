import { NgModule } from '@angular/core';

import { MaquinasMateriaisTreeTableFormComponent } from './maquinas-materiais-tree-table-form.component';
import { VsButtonModule, VsDialogModule, VsFormModule, VsInputModule, VsLabelModule, VsLayoutModule, VsSpinnerModule, VsTextareaModule, VsTreeTableModule } from '@viasoft/components';
import { MaquinasEditorModalComponent } from './maquinas-editor-modal/maquinas-editor-modal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { VsCommonModule } from '@viasoft/common';
import { MAQUINAS_MATERIAIS_TREE_TABLE_FORM_I18N_EN } from './i18n/consts/maquinas-materiais-tree-table-form-i18n-en.consts';
import { MAQUINAS_MATERIAIS_TREE_TABLE_FORM_I18N_PT } from './i18n/consts/maquinas-materiais-tree-table-form-i18n-pt.consts';
import { SharedModule } from '@viasoft/rnc/app/pages/shared/shared.module';
import { MateriaisEditorModalComponent } from './materiais-editor-modal/materiais-editor-modal.component';

@NgModule({
  declarations: [
    MaquinasMateriaisTreeTableFormComponent,
    MaquinasEditorModalComponent,
    MateriaisEditorModalComponent
  ],
  exports: [
    MaquinasMateriaisTreeTableFormComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    VsCommonModule.forChild({
      translates: {
        pt: MAQUINAS_MATERIAIS_TREE_TABLE_FORM_I18N_PT,
        en: MAQUINAS_MATERIAIS_TREE_TABLE_FORM_I18N_EN
      }
    }),
    VsTreeTableModule,
    VsDialogModule,
    VsLabelModule,
    VsFormModule,
    VsInputModule,
    VsTextareaModule,
    VsButtonModule,
    VsSpinnerModule,
    SharedModule,
    VsLayoutModule
  ]
})
export class MaquinasMateriaisTreeTableFormModule { }
