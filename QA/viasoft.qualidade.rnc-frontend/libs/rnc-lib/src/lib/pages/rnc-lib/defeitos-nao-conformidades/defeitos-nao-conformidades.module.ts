import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule, VsGridModule, VsDialogModule, VsLabelModule,
  VsInputModule, VsFormModule, VsSelectModule, VsAutocompleteModule, VsTextareaModule, VsTabGroupModule, VsSpinnerModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';

import { DefeitosNaoConformidadesComponent } from './defeitos-nao-conformidades.component';
import { DefeitosNaoConformidadesEditorModalComponent }
  from './defeitos-nao-conformidades-editor-modal/defeitos-nao-conformidades-editor-modal.component';
import { DEFEITOS_NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/defeitos-nao-conformidades-i18n-en.const';
import { DEFEITOS_NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/defeitos-nao-conformidades-i18n-pt.const';
import { ComponentsModule } from '../../../components/components.module';
import { MatTabsModule } from '@angular/material/tabs';
import { AcoesPreventivasNaoConformidadesModule } from '../acoes-preventivas-nao-conformidades/acoes-preventivas-nao-conformidades.module';
import { CausasNaoConformidadesModule } from '../causas-nao-conformidades/causas-nao-conformidades.module';
import { SolucoesNaoConformidadesModule } from '../solucoes-nao-conformidades/solucoes-nao-conformidades.module';
import { ImplementacaoEvitarReincidenciaNaoConformidadesModule } from '../implementacao-evitar-reincidencia-nao-conformidades/implementacao-evitar-reincidencia-nao-conformidades.module';

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
    ComponentsModule,
    VsTextareaModule,
    VsTabGroupModule,
    MatTabsModule,
    CausasNaoConformidadesModule,
    SolucoesNaoConformidadesModule,
    AcoesPreventivasNaoConformidadesModule,
    ImplementacaoEvitarReincidenciaNaoConformidadesModule,
    VsSpinnerModule
  ]
})
export class DefeitosNaoConformidadesModule { }
