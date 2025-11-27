/* eslint-disable max-len */
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// eslint-disable-next-line max-len
import { UserProxyService } from '@viasoft/rnc/api-clients/Authentication/Users/api/user-proxy.service';
import { VsCommonModule } from '@viasoft/common';
import { VsButtonModule, VsGridModule } from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { ImplementacaoEvitarReincidenciaNaoConformidadesService } from './implementacao-evitar-reincidencia-nao-conformidades.service';
import { IMPLEMENTACAO_EVITAR_REINCIDENCIA_NAO_CONFORMIDADES_I18N_PT } from './i18n/consts/implementacao-evitar-reincidencia-nao-conformidades-i18n-pt.const';
import { IMPLEMENTACAO_EVITAR_REINCIDENCIA_NAO_CONFORMIDADES_I18N_EN } from './i18n/consts/implementacao-evitar-reincidencia-nao-conformidades-i18n-en.const';
import { ImplementacaoEvitarReincidenciaNaoConformidadesComponent } from './implementacao-evitar-reincidencia-nao-conformidades.component';

@NgModule({
  declarations: [ImplementacaoEvitarReincidenciaNaoConformidadesComponent],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: IMPLEMENTACAO_EVITAR_REINCIDENCIA_NAO_CONFORMIDADES_I18N_PT,
        en: IMPLEMENTACAO_EVITAR_REINCIDENCIA_NAO_CONFORMIDADES_I18N_EN
      }
    }),
    CommonModule,
    TabsViewTemplateModule,
    VsButtonModule,
    VsGridModule
  ],
  exports: [
    ImplementacaoEvitarReincidenciaNaoConformidadesComponent
  ],
  providers: [
    ImplementacaoEvitarReincidenciaNaoConformidadesService,
    UserProxyService
  ]
})
export class ImplementacaoEvitarReincidenciaNaoConformidadesModule { }
