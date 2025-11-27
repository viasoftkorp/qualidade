import { NgModule } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';

import { VsButtonModule } from '@viasoft/components/button';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { VsGridModule } from '@viasoft/components/grid';
import { VsDialogModule } from '@viasoft/components/dialog';
import { VsCommonModule } from '@viasoft/common';
import {
  VsCheckboxModule,
  VsDatepickerModule,
  VsFormModule, VsIconModule,
  VsInputModule,
  VsLabelModule,
  VsLayoutModule,
  VsSelectModule,
  VsSpinnerModule,
  VsTextareaModule
} from '@viasoft/components';
import { UserAutocompleteModule } from '@viasoft/administration';
import { PersonAutocompleteSingleModule } from '@viasoft/person-lib';

import { RNC_I18N_PT } from '../../i18n/consts/rnc-i18n-pt.const';
import { RNC_I18N_EN } from '../../i18n/consts/rnc-i18n-en.const';

import { RncEditorModalComponent } from './rnc-editor-modal/rnc-editor-modal.component';
import { RncLibService } from '../../services/rnc-lib.service';
import { RncLibSelectDialogService } from '../../services/rnc-lib-select-dialog.service';
import { ComponentsModule } from '../../components/components.module';
import {
  ConclusaoNaoConformidadesComponent
} from './conclusao-nao-conformidades/conclusao-nao-conformidades.component';
import {
  ConclusaoNaoConformidadesEditorModalComponent
} from './conclusao-nao-conformidades/conclusao-nao-conformidades-editor-modal/conclusao-nao-conformidades-editor-modal.component';
import { NaoConformidadesFilesModule } from './nao-conformidades-files/nao-conformidades-files.module';
import { ProdutosNaoConformidadesModule } from './produtos-nao-conformidades/produtos-nao-conformidades.module';
import {
  ReclamacoesNaoConformidadesComponent
} from './reclamacoes-nao-conformidades/reclamacoes-nao-conformidades.component';
import { DefeitosNaoConformidadesModule } from './defeitos-nao-conformidades/defeitos-nao-conformidades.module';
import { ServicosNaoConformidadesModule } from './servicos-nao-conformidades/servicos-nao-conformidades.module';
import { CausasNaoConformidadesModule } from './causas-nao-conformidades/causas-nao-conformidades.module';
import { SolucoesNaoConformidadesModule } from './solucoes-nao-conformidades/solucoes-nao-conformidades.module';
import {
  AcoesPreventivasNaoConformidadesModule
} from './acoes-preventivas-nao-conformidades/acoes-preventivas-nao-conformidades.module';
import { RncEditorModalService } from './rnc-editor-modal/rnc-editor-modal.service';
import { RncEditorTranslateModule } from './rnc-editor-modal/rnc-editor-translate.module';
import { NotasFiscaisEntradaModule } from '../../components/notas-fiscais-entrada/notas-fiscais-entrada.module';
import { NotasFiscaisSaidaModule } from '../../components/notas-fiscais-saida/notas-fiscais-saida.module';
import { OrdemProducaoModule } from '../../components/ordens-producao/ordem-producao.module';

@NgModule({
  declarations: [
    RncEditorModalComponent,

    ConclusaoNaoConformidadesComponent,
    ConclusaoNaoConformidadesEditorModalComponent,

    ReclamacoesNaoConformidadesComponent,
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: RNC_I18N_PT,
        en: RNC_I18N_EN
      }
    }),
    TabsViewTemplateModule,
    VsButtonModule,
    VsDialogModule,
    VsGridModule,
    VsLayoutModule,
    VsSelectModule,
    VsInputModule,
    VsTextareaModule,
    VsCheckboxModule,
    VsDatepickerModule,
    UserAutocompleteModule,
    PersonAutocompleteSingleModule,
    ComponentsModule,
    VsFormModule,
    MatTabsModule,
    NaoConformidadesFilesModule,
    ProdutosNaoConformidadesModule,
    DefeitosNaoConformidadesModule,
    ServicosNaoConformidadesModule,
    VsLabelModule,
    CausasNaoConformidadesModule,
    SolucoesNaoConformidadesModule,
    AcoesPreventivasNaoConformidadesModule,
    VsIconModule,
    RncEditorTranslateModule,
    OrdemProducaoModule,
    NotasFiscaisEntradaModule,
    NotasFiscaisSaidaModule,
    VsSpinnerModule
  ],
  providers: [
    RncLibService,
    RncLibSelectDialogService,
    RncEditorModalService,
  ],
  exports: [
    RncEditorModalComponent
  ]
})
export class RncLibModule {
}
