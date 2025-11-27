import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule, VsDialogModule, VsGridModule, VsInputModule, VsLabelModule, VsSpinnerModule, VsTableModule, VsTextareaModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { SharedModule } from '../../shared/shared.module';
import { DefeitoComponent } from './defeito.component';
import { DefeitoEditorModalComponent } from './defeito-editor-modal/defeito-editor-modal.component';
import { DEFEITO_I18N_EN } from './i18n/consts/defeito-i18n-en.const';
import { DEFEITO_I18N_PT } from './i18n/consts/defeito-i18n-pt.const';
import { DefeitoRoutingModule } from './defeito-routing.module';
import { CAUSAS_PROXY_URL } from '../../shared/causa-autocomplete-select/causa-provider/tokens';

@NgModule({
  declarations: [
    DefeitoComponent,
    DefeitoEditorModalComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: DEFEITO_I18N_PT,
        en: DEFEITO_I18N_EN
      }
    }),
    CommonModule,
    DefeitoRoutingModule,
    VsCommonModule,
    VsDialogModule,
    VsLabelModule,
    VsInputModule,
    VsButtonModule,
    VsTableModule,
    TabsViewTemplateModule,
    VsGridModule,
    VsTextareaModule,
    SharedModule,
    VsSpinnerModule
  ],
  providers: [
    { provide: CAUSAS_PROXY_URL, useValue: 'qualidade/rnc/gateway/causas' },
  ]
})
export class DefeitoModule { }
