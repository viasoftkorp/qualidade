import { VsCommonModule } from "@viasoft/common";
import { NgModule } from "@angular/core";
import { ConfiguracoesRoutingModule } from "./configuracoes-routing.module";

@NgModule({
  declarations: [
  ],
  imports: [
    VsCommonModule.forChild({
    }),
    ConfiguracoesRoutingModule
  ],
  providers: [
  ]
})
export class ConfiguracoesModule { }
