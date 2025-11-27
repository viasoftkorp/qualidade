import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { VsFormModule, VsInputModule, VsLayoutModule } from '@viasoft/components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VsCommonModule } from '@viasoft/common';
import { PedidoVendaNaoConformidadesComponent } from './pedido-venda-nao-conformidades.component';
import { SharedModule } from '../../../shared/shared.module';
import { PEDIDO_VENDA_NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/pedido-venda-nao-conformidades-i18n-pt.const';
import { PEDIDO_VENDA_NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/pedido-venda-nao-conformidades-i18n-en.const';
import { PedidoVendaModule } from '../../../shared/pedido-venda/pedido-venda.module';
import { CentroCustoModule } from '../../../shared/centro-custo/centro-custo.module';

@NgModule({
  declarations: [
    PedidoVendaNaoConformidadesComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: PEDIDO_VENDA_NAO_CONFORMIDADE_I18N_PT,
        en: PEDIDO_VENDA_NAO_CONFORMIDADE_I18N_EN
      }
    }),
    CommonModule,
    TabsViewTemplateModule,
    VsLayoutModule,
    VsFormModule,
    ReactiveFormsModule,
    FormsModule,
    VsInputModule,
    SharedModule,
    PedidoVendaModule,
    CentroCustoModule
  ],
  exports: [
    PedidoVendaNaoConformidadesComponent
  ]
})
export class PedidoVendaNaoConformidadesModule { }
