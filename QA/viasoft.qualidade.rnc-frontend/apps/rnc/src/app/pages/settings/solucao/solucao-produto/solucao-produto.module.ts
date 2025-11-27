import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  SolucaoProdutoComponent
} from '@viasoft/rnc/app/pages/settings/solucao/solucao-produto/solucao-produto.component';
import {
  VsAutocompleteModule,
  VsButtonModule,
  VsDialogModule,
  VsFormModule,
  VsGridModule,
  VsInputModule,
  VsLabelModule, VsSelectModule, VsTextareaModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { VsCommonModule } from '@viasoft/common';
import { SharedModule } from '../../../shared/shared.module';
import { SolucaoProdutoEditorModalComponent } from './solucao-produto-editor-modal/solucao-produto-editor-modal.component';
import { SOLUCAO_PRODUTO_I18N_PT } from './i18n/consts/solucao-produto-i18n-pt.const';
import { SOLUCAO_PRODUTO_I18N_EN } from './i18n/consts/solucao-produto-i18n-en.const';

@NgModule({
  declarations: [
    SolucaoProdutoComponent,
    SolucaoProdutoEditorModalComponent,
  ],
  exports: [
    SolucaoProdutoComponent
  ],
  imports: [
    CommonModule,
    VsCommonModule.forChild({
      translates: {
        pt: SOLUCAO_PRODUTO_I18N_PT,
        en: SOLUCAO_PRODUTO_I18N_EN
      }
    }),
    VsButtonModule,
    VsGridModule,
    TabsViewTemplateModule,
    VsDialogModule,
    VsLabelModule,
    VsInputModule,
    VsFormModule,
    VsCommonModule,
    VsSelectModule,
    VsAutocompleteModule,
    SharedModule,
    VsTextareaModule
  ]
})
export class SolucaoProdutoModule { }
