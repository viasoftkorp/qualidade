import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import { VsButtonModule, VsGridModule, VsHeaderModule, VsLabelModule } from '@viasoft/components';
import { VsIconModule } from '@viasoft/components/icon';
import { VsLayoutModule } from '@viasoft/components/layout';
import { CONTROLE_TRATAMENTO_TERMICO_I18N_PT } from './i18n/consts/controle-tratamento-termico-i18n-pt.const';
import { ControleTratamentoTermicoRoutingModule } from './controle-tratamento-termico-routing.module';
import { ControleTratamentoTermicoComponent } from './controle-tratamento-termico.component';
import { TabsViewTemplateModule } from '@viasoft/view-template';

@NgModule({
  declarations: [ControleTratamentoTermicoComponent],
  imports: [
    VsCommonModule.forChild({ translates: { pt: CONTROLE_TRATAMENTO_TERMICO_I18N_PT } }),
    ControleTratamentoTermicoRoutingModule,
    VsButtonModule,
    VsGridModule,
    VsHeaderModule,
    VsIconModule,
    VsLayoutModule,
    VsLabelModule,
    TabsViewTemplateModule
  ]
})
export class ControleTratamentoTermicoModule { }
