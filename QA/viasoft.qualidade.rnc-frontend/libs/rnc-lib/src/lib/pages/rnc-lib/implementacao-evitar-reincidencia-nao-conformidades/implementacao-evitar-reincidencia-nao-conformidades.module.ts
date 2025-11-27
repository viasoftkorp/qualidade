import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { VsCommonModule } from "@viasoft/common";
import { VsButtonModule, VsGridModule } from "@viasoft/components";
import { TabsViewTemplateModule } from "@viasoft/view-template";
import { IMPLEMENTACAO_EVITAR_REINCIDENCIA_NAO_CONFORMIDADES_I18N_EN } from "./i18n/consts/implementacao-evitar-reincidencia-nao-conformidades-i18n-en.const";
import { IMPLEMENTACAO_EVITAR_REINCIDENCIA_NAO_CONFORMIDADES_I18N_PT } from "./i18n/consts/implementacao-evitar-reincidencia-nao-conformidades-i18n-pt.const";
import { ImplementacaoEvitarReincidenciaNaoConformidadesComponent } from "./implementacao-evitar-reincidencia-nao-conformidades.component";
import { ImplementacaoEvitarReincidenciaNaoConformidadesService } from "./implementacao-evitar-reincidencia-nao-conformidades.service";
import { ImplementacaoEvitarReincidenciaNaoConformidadesEditorModalModule } from "./implementacao-evitar-reincidencia-nao-conformidades-editor-modal/implementacao-evitar-reincidencia-nao-conformidades-editor-modal.module";
import { UserProxyService } from "../../../api-clients/Authentication/Users/api/user-proxy.service";

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
    VsGridModule,
    ImplementacaoEvitarReincidenciaNaoConformidadesEditorModalModule
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
