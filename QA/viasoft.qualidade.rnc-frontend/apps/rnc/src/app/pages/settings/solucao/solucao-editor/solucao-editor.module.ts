import { TabsViewTemplateModule } from '@viasoft/view-template';
import {
  VsButtonModule,
  VsCheckboxModule,
  VsFormModule,
  VsGridModule,
  VsInputModule,
  VsLabelModule,
  VsLayoutModule,
  VsSpinnerModule,
  VsTextareaModule
} from '@viasoft/components';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { VsCommonModule } from '@viasoft/common';
import { SolucaoProdutoModule } from '@viasoft/rnc/app/pages/settings/solucao/solucao-produto/solucao-produto.module';
import { SolucaoServicoModule } from '@viasoft/rnc/app/pages/settings/solucao/solucao-servico/solucao-servico.module';
import { SolucaoEditorRoutingModule } from './solucao-editor-routing.module';
import { SolucaoEditorComponent } from './solucao-editor.component';
import { SOLUCAO_EDITOR_I18N_PT } from './i18n/consts/solucao-editor-i18n-pt.const';
import { SOLUCAO_EDITOR_I18N_EN } from './i18n/consts/solucao-editor-i18n-en.const';

@NgModule({
  declarations: [
    SolucaoEditorComponent,
  ],
  imports: [
    CommonModule,
    VsCommonModule.forChild({
      translates: {
        pt: SOLUCAO_EDITOR_I18N_PT,
        en: SOLUCAO_EDITOR_I18N_EN
      }
    }),
    SolucaoEditorRoutingModule,
    VsButtonModule,
    TabsViewTemplateModule,
    VsGridModule,
    VsLabelModule,
    VsInputModule,
    VsCheckboxModule,
    MatTabsModule,
    VsFormModule,
    VsLayoutModule,
    VsCommonModule,
    VsTextareaModule,
    SolucaoProdutoModule,
    SolucaoServicoModule,
    VsSpinnerModule
  ]
})
export class SolucaoEditorModule { }
