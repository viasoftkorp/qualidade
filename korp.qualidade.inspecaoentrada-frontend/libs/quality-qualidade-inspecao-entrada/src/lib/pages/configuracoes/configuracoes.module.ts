import { NgModule } from '@angular/core';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { VsButtonModule, VsDialogModule, VsFormModule, VsGridModule, VsInputModule, VsLabelModule } from '@viasoft/components';
import { VsCommonModule } from '@viasoft/common';

import { PlanoAmostragemComponent } from './plano-amostragem/plano-amostragem.component';
import { ConfiguracoesRoutingModule } from './configuracoes-routing.module';
import { CONFIGURACOES_I18N_PT } from './i18n/consts/configuracoes-i18-pt';
import { ConfiguracoesService } from './configuracoes.service';
import { PlanoAmostragemEditorComponent } from './plano-amostragem/plano-amostragem-editor/plano-amostragem-editor.component';

@NgModule({
  declarations: [
    PlanoAmostragemComponent,
    PlanoAmostragemEditorComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: { pt: CONFIGURACOES_I18N_PT }
    }),
    ConfiguracoesRoutingModule,
    TabsViewTemplateModule,
    VsButtonModule,
    VsGridModule,
    VsFormModule,
    VsInputModule,
    VsLabelModule,
    VsDialogModule
  ],
  providers: [
    ConfiguracoesService
  ]
})
export class ConfiguracoesModule { }