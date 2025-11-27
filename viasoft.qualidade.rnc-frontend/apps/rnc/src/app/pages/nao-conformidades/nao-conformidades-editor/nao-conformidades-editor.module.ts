import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { UserSelectService, UserModule, UserAutocompleteModule } from '@viasoft/administration';
import { VsCommonModule, VsUserService } from '@viasoft/common';
import {
  VsButtonModule,
  VsGridModule,
  VsLabelModule,
  VsInputModule,
  VsCheckboxModule,
  VsFormModule,
  VsLayoutModule,
  VsTextareaModule,
  VsSelectModule,
  VsDatepickerModule,
  VsDialogModule,
  VsSpinnerModule
} from '@viasoft/components';
import { VsFileProviderModule } from '@viasoft/file-provider';
import {
  PersonAutocompleteSingleModule, PersonSelectModule, PersonService, PersonServiceProxy, OtherAddressServiceProxy
} from '@viasoft/person-lib';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { SharedModule } from '../../shared/shared.module';
import { NaoConformidadesFilesModule } from '../nao-conformidades-files/nao-conformidades-files.module';
import { NaoConformidadesEditorRoutingModule } from './nao-conformidades-editor-routing.module';
import { NaoConformidadesEditorComponent } from './nao-conformidades-editor.component';
import { ConclusaoNaoConformidadesEditorModalComponent } from './conclusao-nao-conformidades';
import { ConclusaoNaoConformidadesComponent } from './conclusao-nao-conformidades/conclusao-nao-conformidades.component';
import { ReclamacoesNaoConformidadesComponent }
  from './reclamacoes-nao-conformidades/reclamacoes-nao-conformidades.component';
import { RelatorioNaoConformidadesService } from './relatorio-nao-conformidades/relatorio-nao-conformidades.service';
import { ProdutosNaoConformidadesModule } from './produtos-nao-conformidades/produtos-nao-conformidades.module';
import { ServicosNaoConformidadesModule } from './servicos-nao-conformidades/servicos-nao-conformidades.module';
import { NotasFiscaisEntradaModule } from '../../shared/notas-fiscais-entrada/notas-fiscais-entrada.module';
import { NotasFiscaisSaidaModule } from '../../shared/notas-fiscais-saida/notas-fiscais-saida.module';
import { PedidoVendaNaoConformidadesModule } from './pedido-venda-nao-conformidades/pedido-venda-nao-conformidades.module';
import { DefeitosNaoConformidadesModule } from './defeitos-nao-conformidades/defeitos-nao-conformidades.module';
import { OrdemProducaoModule } from '../../shared/ordens-producao/ordem-producao.module';
import { RetrabalhoNaoConformidadesModule } from './retrabalho-nao-conformidades/retrabalho-nao-conformidades.module';
import { OdfRetrabalhoNaoConformidadesModule } from './retrabalho-nao-conformidades/odf-retrabalho-nao-conformidades';
import { OperacaoRetrabalhoNaoConformidadesModule } from './retrabalho-nao-conformidades';
import { ConclusaoNaoConformidadesService } from './conclusao-nao-conformidades/conclusao-nao-conformidades.service';

@NgModule({
  declarations: [NaoConformidadesEditorComponent,
    ReclamacoesNaoConformidadesComponent,
    ConclusaoNaoConformidadesEditorModalComponent,
    ConclusaoNaoConformidadesComponent],
  imports: [
    CommonModule,
    NaoConformidadesEditorRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    VsButtonModule,
    TabsViewTemplateModule,
    VsDialogModule,
    VsGridModule,
    VsLabelModule,
    VsInputModule,
    VsCheckboxModule,
    MatTabsModule,
    VsFormModule,
    VsLayoutModule,
    VsCommonModule,
    VsTextareaModule,
    VsSelectModule,
    PersonAutocompleteSingleModule,
    PersonSelectModule,
    VsDatepickerModule,
    SharedModule,
    UserModule,
    UserAutocompleteModule,
    DefeitosNaoConformidadesModule,
    NaoConformidadesFilesModule,
    VsFileProviderModule,
    ProdutosNaoConformidadesModule,
    ServicosNaoConformidadesModule,
    OrdemProducaoModule,
    NotasFiscaisEntradaModule,
    NotasFiscaisSaidaModule,
    PedidoVendaNaoConformidadesModule,
    VsSpinnerModule,
    RetrabalhoNaoConformidadesModule,
    OperacaoRetrabalhoNaoConformidadesModule,
    OdfRetrabalhoNaoConformidadesModule
  ],
  providers: [
    PersonService,
    PersonServiceProxy,
    OtherAddressServiceProxy,
    UserSelectService,
    VsUserService,
    RelatorioNaoConformidadesService,
    ConclusaoNaoConformidadesService
  ]
})
export class NaoConformidadesEditorModule { }
