import { NgModule } from '@angular/core';
import {ConfiguracoesComponent} from "./configuracoes.component";
import {VsCommonModule} from "@viasoft/common";

import {CONFIGURACOES_I18N_PT} from "./i18n/consts/qualidade-inspecao-saida-i18n-pt.const";
import {ConfiguracoesRoutingModule} from "./configuracoes-routing.module";
import {HistoricoInspecaoModule} from "../qualidade-inspecao-saida/historico/historico-inspecao.module";
import {TabsViewTemplateModule} from "@viasoft/view-template";
import {VsButtonModule, VsCheckboxModule, VsFormModule, VsLayoutModule, VsTabGroupModule} from "@viasoft/components";
import {ConfiguracoesService} from "./configuracoes.service";

@NgModule({
  declarations: [
    ConfiguracoesComponent
  ],
  imports: [
    ConfiguracoesRoutingModule,
    VsCommonModule.forChild({
      translates: {
        pt: CONFIGURACOES_I18N_PT
      }
    }),
    HistoricoInspecaoModule,
    TabsViewTemplateModule,
    VsTabGroupModule,
    VsButtonModule,
    VsLayoutModule,
    VsFormModule,
    VsCheckboxModule,
  ]
})
export class ConfiguracoesModule { }
