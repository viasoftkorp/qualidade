import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule,
  VsGridModule,
  VsDialogModule,
  VsLabelModule,
  VsInputModule,
  VsTableModule,
  VsFormModule,
  VsTextareaModule,
  VsLayoutModule,
  VsIconModule,
  VsSpinnerModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { UserSelectService } from '@viasoft/administration';
import { ProdutosNaoConformidadesComponent } from './produtos-nao-conformidades.component';
// eslint-disable-next-line max-len
import { ProdutosNaoConformidadesEditorModalComponent } from './produtos-nao-conformidades-editor-modal/produtos-nao-conformidades-editor-modal.component';
import { SharedModule } from '../../../shared/shared.module';
import { PRODUTOS_NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/produtos-nao-conformidades-i18n-pt.const';
import { PRODUTOS_NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/produtos-nao-conformidades-i18n-en.const';

@NgModule({
  declarations: [
    ProdutosNaoConformidadesComponent,
    ProdutosNaoConformidadesEditorModalComponent
  ],
  imports: [
    CommonModule,
    VsCommonModule.forChild({
      translates: {
        pt: PRODUTOS_NAO_CONFORMIDADE_I18N_PT,
        en: PRODUTOS_NAO_CONFORMIDADE_I18N_EN
      }
    }),
    VsCommonModule,
    VsDialogModule,
    VsLabelModule,
    VsInputModule,
    VsButtonModule,
    VsTableModule,
    TabsViewTemplateModule,
    VsGridModule,
    VsFormModule,
    FormsModule,
    ReactiveFormsModule,
    VsTextareaModule,
    SharedModule,
    VsLayoutModule,
    VsIconModule,
    VsSpinnerModule
  ],
  providers: [UserSelectService],
  exports: [
    ProdutosNaoConformidadesComponent
  ]
})
export class ProdutosNaoConformidadesModule { }
