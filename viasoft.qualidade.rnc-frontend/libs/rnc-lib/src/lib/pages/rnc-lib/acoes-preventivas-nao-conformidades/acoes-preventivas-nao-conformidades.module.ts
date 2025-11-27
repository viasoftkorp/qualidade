import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule, VsGridModule, VsDialogModule, VsLabelModule, VsInputModule,
  VsFormModule, VsSelectModule, VsAutocompleteModule, VsTextareaModule, VsDatepickerModule, VsCheckboxModule, VsSpinnerModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { UserAutocompleteModule, UserModule, UserSelectService } from '@viasoft/administration';
import { AcoesPreventivasNaoConformidadesComponent } from './acoes-preventivas-nao-conformidades.component';
import { AcoesPreventivasNaoConformidadesEditorModalComponent }
  from './acoes-preventivas-nao-conformidades-editor-modal/acoes-preventivas-nao-conformidades-editor-modal.component';import { ACOES_PREVENTIVAS_NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/acoes-preventivas-nao-conformidades-i18n-en.const';
import { ACOES_PREVENTIVAS_NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/acoes-preventivas-nao-conformidades-i18n-pt.const';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  declarations: [
    AcoesPreventivasNaoConformidadesComponent,
    AcoesPreventivasNaoConformidadesEditorModalComponent
  ],
  imports: [
    CommonModule,
    VsCommonModule.forChild({
      translates: {
        pt: ACOES_PREVENTIVAS_NAO_CONFORMIDADE_I18N_PT,
        en: ACOES_PREVENTIVAS_NAO_CONFORMIDADE_I18N_EN
      }
    }),
    VsButtonModule,
    VsGridModule,
    TabsViewTemplateModule,
    VsDialogModule,
    VsLabelModule,
    VsInputModule,
    VsFormModule,
    VsSelectModule,
    VsAutocompleteModule,
    ComponentsModule,
    VsTextareaModule,
    UserModule,
    UserAutocompleteModule,
    VsDatepickerModule,
    VsCheckboxModule,
    VsSpinnerModule
  ],
  exports: [AcoesPreventivasNaoConformidadesComponent],
  providers: [UserSelectService]
})
export class AcoesPreventivasNaoConformidadesModule { }
