import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  VsButtonModule,
  VsDialogModule,
  VsFormModule,
  VsGridModule,
  VsInputModule,
  VsLabelModule,
  VsLayoutModule,
  VsSpinnerModule,
  VsTreeTableModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
// eslint-disable-next-line max-len
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@viasoft/rnc/app/pages/shared/shared.module';
import { TranslatePipe } from '@ngx-translate/core';
import { OperacaoRetrabalhoNaoConformidadesComponent } from './operacao-retrabalho-nao-conformidades.component';
// eslint-disable-next-line max-len
import { GerarOperacaoRetrabalhoEditorModalComponent } from './gerar-operacao-retrabalho-editor-modal/gerar-operacao-retrabalho-editor-modal.component';
// eslint-disable-next-line max-len
import { MaquinasMateriaisTreeTableFormModule } from './gerar-operacao-retrabalho-editor-modal/maquinas-materiais-tree-table-form/maquinas-materiais-tree-table-form.module';

@NgModule({
  declarations: [
    OperacaoRetrabalhoNaoConformidadesComponent,
    GerarOperacaoRetrabalhoEditorModalComponent,
  ],
  exports: [
    OperacaoRetrabalhoNaoConformidadesComponent
  ],
  imports: [
    CommonModule,
    TabsViewTemplateModule,
    VsLayoutModule,
    VsFormModule,
    ReactiveFormsModule,
    FormsModule,
    VsButtonModule,
    VsLabelModule,
    VsSpinnerModule,
    VsDialogModule,
    VsInputModule,
    VsGridModule,
    SharedModule,
    VsTreeTableModule,
    MaquinasMateriaisTreeTableFormModule,
    VsButtonModule,
    VsTreeTableModule
  ],
  providers: [
    TranslatePipe
  ]
})
export class OperacaoRetrabalhoNaoConformidadesModule { }
