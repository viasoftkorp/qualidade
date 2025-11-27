import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  VsButtonModule,
  VsLabelModule,
  VsSpinnerModule,
  VsDialogModule,
  VsFormModule,
  VsInputModule
} from '@viasoft/components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VsCommonModule } from '@viasoft/common';
import { RetrabalhoNaoConformidadesService } from './retrabalho-nao-conformidades.service';
import { GerarRetrabalhoButtonComponent } from './gerar-retrabalho-button.component';
// eslint-disable-next-line max-len
import { OperacaoRetrabalhoNaoConformidadesModule } from './operacao-retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades.module';
import { EstoqueLocalModule } from '../../../shared/estoque-local/estoque-local.module';
// eslint-disable-next-line max-len
import { EstoquePedidoVendaEstoqueLocalModule } from '../../../shared/estoque-pedido-venda-estoque-local/estoque-pedido-venda-estoque-local.module';
import { LocalModule } from '../../../shared/local/local.module';
import { RETRABALHO_NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/retrabalho-nao-conformidades-i18n-pt.consts';
import { RETRABALHO_NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/retrabalho-nao-conformidades-i18n-en.consts';
import { OdfRetrabalhoNaoConformidadesModule } from './odf-retrabalho-nao-conformidades/odf-retrabalho-nao-conformidades.module';

@NgModule({
  declarations: [
    GerarRetrabalhoButtonComponent,
  ],
  exports: [
    GerarRetrabalhoButtonComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: RETRABALHO_NAO_CONFORMIDADE_I18N_PT,
        en: RETRABALHO_NAO_CONFORMIDADE_I18N_EN
      }
    }),
    CommonModule,
    OperacaoRetrabalhoNaoConformidadesModule,
    OdfRetrabalhoNaoConformidadesModule,
    VsButtonModule,
    VsLabelModule,
    VsSpinnerModule,
    VsDialogModule,
    FormsModule,
    VsFormModule,
    ReactiveFormsModule,
    LocalModule,
    EstoqueLocalModule,
    EstoquePedidoVendaEstoqueLocalModule,
    VsInputModule,
  ],
  providers: [RetrabalhoNaoConformidadesService],
})
export class RetrabalhoNaoConformidadesModule {}
