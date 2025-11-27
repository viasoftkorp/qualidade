import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// eslint-disable-next-line max-len
import {
  VsButtonModule,
  VsDialogModule,
  VsFormModule,
  VsInputModule,
  VsLabelModule,
  VsLayoutModule,
  VsSpinnerModule,
} from '@viasoft/components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EstoqueLocalModule } from '@viasoft/rnc/app/pages/shared/estoque-local/estoque-local.module';
// eslint-disable-next-line max-len
import { EstoquePedidoVendaEstoqueLocalModule } from '@viasoft/rnc/app/pages/shared/estoque-pedido-venda-estoque-local/estoque-pedido-venda-estoque-local.module';
import { LocalModule } from '@viasoft/rnc/app/pages/shared/local/local.module';
// eslint-disable-next-line max-len
import { TabsViewTemplateModule } from '@viasoft/view-template';
// eslint-disable-next-line max-len
import { VsCommonModule } from '@viasoft/common';
import { GerarOdfRetrabalhoEditorModalComponent } from './gerar-odf-retrabalho-editor-modal/gerar-odf-retrabalho-editor-modal.component';
import { OdfRetrabalhoNaoConformidadesComponent } from './odf-retrabalho-nao-conformidades.component';
import { ODF_RETRABALHO_NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/odf-retrabalho-nao-conformidades-i18n-en.consts';
import { ODF_RETRABALHO_NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/odf-retrabalho-nao-conformidades-i18n-pt.consts';

@NgModule({
  declarations: [GerarOdfRetrabalhoEditorModalComponent, OdfRetrabalhoNaoConformidadesComponent],
  exports: [GerarOdfRetrabalhoEditorModalComponent, OdfRetrabalhoNaoConformidadesComponent],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: ODF_RETRABALHO_NAO_CONFORMIDADE_I18N_PT,
        en: ODF_RETRABALHO_NAO_CONFORMIDADE_I18N_EN
      }
    }),
    CommonModule,
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
    VsLayoutModule,
    TabsViewTemplateModule,
  ],
})
export class OdfRetrabalhoNaoConformidadesModule {}
