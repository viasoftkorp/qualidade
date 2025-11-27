import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  VsAutocompleteModule, VsButtonModule, VsCheckboxModule, VsDatepickerModule, VsDialogModule, VsFormModule,
  VsGridModule, VsInputModule, VsLabelModule, VsLayoutModule, VsSelectModule, VsSpinnerModule, VsTextareaModule
} from '@viasoft/components';
import { VsCommonModule } from '@viasoft/common';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { UserAutocompleteModule, UserModule, UserSelectService } from '@viasoft/administration';
import { MatTabsModule } from '@angular/material/tabs';
import { SharedModule } from '@viasoft/rnc/app/pages/shared/shared.module';
import { SolucoesNaoConformidadesComponent } from './solucoes-nao-conformidades.component';
import { SOLUCOES_NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/solucoes-nao-conformidades-i18n-en.const';
import { SOLUCOES_NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/solucoes-nao-conformidades-i18n-pt.const';
import { SolucoesNaoConformidadesEditorModalComponent }
  from './solucoes-nao-conformidades-editor-modal/solucoes-nao-conformidades-editor-modal.component';

@NgModule({
  declarations: [SolucoesNaoConformidadesComponent, SolucoesNaoConformidadesEditorModalComponent],
  imports: [
    CommonModule,
    VsCommonModule.forChild({
      translates: {
        pt: SOLUCOES_NAO_CONFORMIDADE_I18N_PT,
        en: SOLUCOES_NAO_CONFORMIDADE_I18N_EN
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
    VsCheckboxModule,
    UserModule,
    UserAutocompleteModule,
    VsLayoutModule,
    VsDatepickerModule,
    MatTabsModule,
    VsSpinnerModule
  ],
  exports: [SolucoesNaoConformidadesComponent],
  providers: [
    UserSelectService
  ]
})
export class SolucoesNaoConformidadesModule { }
