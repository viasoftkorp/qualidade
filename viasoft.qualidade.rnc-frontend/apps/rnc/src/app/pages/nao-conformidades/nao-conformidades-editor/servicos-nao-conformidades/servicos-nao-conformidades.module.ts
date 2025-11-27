import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { VsCommonModule, UserModule } from '@viasoft/common';
import {
  VsButtonModule, VsGridModule, VsDialogModule, VsLabelModule, VsInputModule,
  VsFormModule, VsSelectModule, VsAutocompleteModule, VsTextareaModule, VsCheckboxModule, VsLayoutModule, VsDatepickerModule, VsIconModule, VsSpinnerModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { MatDialogModule } from '@angular/material/dialog';
import { SharedModule } from '../../../shared/shared.module';
import { ServicosNaoConformidadesComponent } from './servicos-nao-conformidades.component';
// eslint-disable-next-line max-len
import { ServicosNaoConformidadesEditorModalComponent } from './servicos-nao-conformidades-editor-modal/servicos-nao-conformidades-editor-modal.component';
import { SERVICOS_NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/servicos-nao-conformidades-i18n-pt.const';
import { SERVICOS_NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/servicos-nao-conformidades-i18n-en.const';

@NgModule({
  declarations: [ServicosNaoConformidadesComponent, ServicosNaoConformidadesEditorModalComponent],
  imports: [
    CommonModule,
    VsCommonModule.forChild({
      translates: {
        pt: SERVICOS_NAO_CONFORMIDADE_I18N_PT,
        en: SERVICOS_NAO_CONFORMIDADE_I18N_EN
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
    VsLayoutModule,
    VsDatepickerModule,
    MatTabsModule,
    MatDialogModule,
    VsIconModule,
    VsSpinnerModule
  ],
  exports: [ServicosNaoConformidadesComponent]
})
export class ServicosNaoConformidadesModule { }
