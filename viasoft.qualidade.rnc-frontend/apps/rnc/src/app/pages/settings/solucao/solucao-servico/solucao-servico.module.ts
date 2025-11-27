import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  SolucaoServicoComponent
} from '@viasoft/rnc/app/pages/settings/solucao/solucao-servico/solucao-servico.component';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule, VsGridModule,
  VsDialogModule, VsLabelModule, VsInputModule,
  VsFormModule, VsSelectModule, VsAutocompleteModule, VsTextareaModule
}
  from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { SolucaoServicoEditorModalComponent } from './solucao-servico-editor-modal/solucao-servico-editor-modal.component';
import { SOLUCAO_SERVICO_I18N_EN } from './i18n/consts/solucao-servico-i18n-en.const';
import { SOLUCAO_SERVICO_I18N_PT } from './i18n/consts/solucao-servico-i18n-pt.const';
import { SharedModule } from '../../../shared/shared.module';

@NgModule({
  declarations: [
    SolucaoServicoComponent,
    SolucaoServicoEditorModalComponent,
  ],
  exports: [
    SolucaoServicoComponent,
  ],
  imports: [
    CommonModule,
    VsCommonModule.forChild({
      translates: {
        pt: SOLUCAO_SERVICO_I18N_PT,
        en: SOLUCAO_SERVICO_I18N_EN
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
    VsTextareaModule,
    SharedModule
  ]
})
export class SolucaoServicoModule { }
