import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule, VsGridModule, VsDialogModule, VsLabelModule,
  VsInputModule, VsFormModule, VsSelectModule, VsAutocompleteModule, VsTextareaModule, VsSpinnerModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';

import { MatTabsModule } from '@angular/material/tabs';
import { DefeitosNaoConformidadesComponent } from './defeitos-nao-conformidades.component';
import { DefeitosNaoConformidadesEditorModalComponent }
  from './defeitos-nao-conformidades-editor-modal/defeitos-nao-conformidades-editor-modal.component';
import { DEFEITOS_NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/defeitos-nao-conformidades-i18n-en.const';
import { DEFEITOS_NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/defeitos-nao-conformidades-i18n-pt.const';
import { SharedModule } from '../../../shared/shared.module';
import { CausasNaoConformidadesModule } from './causas-nao-conformidades/causas-nao-conformidades.module';
import { SolucoesNaoConformidadesModule } from './solucoes-nao-conformidades/solucoes-nao-conformidades.module';
// eslint-disable-next-line max-len
import { AcoesPreventivasNaoConformidadesModule } from './acoes-preventivas-nao-conformidades/acoes-preventivas-nao-conformidades.module';
// eslint-disable-next-line max-len
import { ImplementacaoEvitarReincidenciaNaoConformidadesModule } from './implementacao-evitar-reincidencia-nao-conformidades/implementacao-evitar-reincidencia-nao-conformidades.module';

@NgModule({
  declarations: [DefeitosNaoConformidadesComponent, DefeitosNaoConformidadesEditorModalComponent],
  exports: [DefeitosNaoConformidadesComponent],
  imports: [
    CommonModule,
    VsCommonModule.forChild({
      translates: {
        pt: DEFEITOS_NAO_CONFORMIDADE_I18N_PT,
        en: DEFEITOS_NAO_CONFORMIDADE_I18N_EN
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
    VsTextareaModule,
    MatTabsModule,
    CausasNaoConformidadesModule,
    SolucoesNaoConformidadesModule,
    AcoesPreventivasNaoConformidadesModule,
    ImplementacaoEvitarReincidenciaNaoConformidadesModule,
    VsSpinnerModule
  ]
})
export class DefeitosNaoConformidadesModule { }
