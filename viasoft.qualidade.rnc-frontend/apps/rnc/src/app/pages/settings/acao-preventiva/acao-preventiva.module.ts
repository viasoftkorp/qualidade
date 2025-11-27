import { VsCommonModule } from '@viasoft/common';
import { UserAutocompleteModule, UserModule } from '@viasoft/administration';
import {
  VsButtonModule,
  VsDialogModule,
  VsGridModule,
  VsInputModule,
  VsLabelModule,
  VsSpinnerModule,
  VsTextareaModule,
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { NgModule } from '@angular/core';

import { UserProxyService } from '@viasoft/rnc/api-clients/Authentication/Users/api/user-proxy.service';
import { ACAO_PREVENTIVA_I18N_EN } from './i18n/consts/acao-preventiva-i18n-en.consts';
import { ACAO_PREVENTIVA_I18N_PT } from './i18n/consts/acao-preventiva-i18n-pt.consts';

import { AcaoPreventivaComponent } from './acao-preventiva.component';
import { AcaoPreventivaRoutingModule } from './acao-preventiva-routing.module';
import {
  AcaoPreventivaEditorModalComponent
} from './acao-preventiva-editor-modal/acao-preventiva-editor-modal/acao-preventiva-editor-modal.component';

@NgModule({
  declarations: [
    AcaoPreventivaComponent,
    AcaoPreventivaEditorModalComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: ACAO_PREVENTIVA_I18N_PT,
        en: ACAO_PREVENTIVA_I18N_EN
      }
    }),
    VsCommonModule,
    VsGridModule,
    TabsViewTemplateModule,
    VsButtonModule,
    VsDialogModule,
    VsLabelModule,
    VsInputModule,
    VsTextareaModule,
    UserModule,
    UserAutocompleteModule,
    AcaoPreventivaRoutingModule,
    VsSpinnerModule
  ],
  providers: [
    UserProxyService
  ]
})
export class AcaoPreventivaModule { }
