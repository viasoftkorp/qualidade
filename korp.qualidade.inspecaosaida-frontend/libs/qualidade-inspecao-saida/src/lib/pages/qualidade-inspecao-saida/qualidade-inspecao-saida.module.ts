import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule,
  VsCheckboxModule,
  VsChipListModule,
  VsDatepickerModule,
  VsDialogModule,
  VsFormModule,
  VsGridModule,
  VsIconModule,
  VsInputModule,
  VsLabelModule,
  VsLayoutModule,
  VsSelectModule,
  VsTabGroupModule,
  VsTextareaModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { RncLibModule } from '@viasoft/rnc-lib';
import { QualidadeInspecaoSaidaService } from '../../services/qualidade-inspecao-saida.service';
import { QUALIDADE_INSPECAO_SAIDA_I18N_PT } from './i18n/consts/qualidade-inspecao-saida-i18n-pt.const';
import {
  FinalizarInspecaoModalComponent
} from './inspecao-details/finalizar-inspecao-modal/finalizar-inspecao-modal.component';
import { InspecaoDetailsComponent } from './inspecao-details/inspecao-details.component';
import { QualidadeInspecaoSaidaRoutingModule } from './qualidade-inspecao-saida-routing.module';
import { QualidadeInspecaoSaidaComponent } from './qualidade-inspecao-saida.component';
import { OrdemProducaoViewComponent } from './ordem-producao-view/ordem-producao-view.component';
import { InspecaoViewComponent } from './inspecao-view/inspecao-view.component';
import {
  AlterarDadosInspecaoModalComponent
} from './inspecao-details/alterar-dados-inspecao-modal/alterar-dados-inspecao-modal.component';
import { ComponentsModule } from '../../components/components.module';
import { PersonLibStartupModule } from '@viasoft/person-lib';
import { FileProviderTagStartupModule } from '@viasoft/custom-file-provider-tag';
import { HistoricoInspecaoModule } from './historico/historico-inspecao.module';
import { InspecoesAbertasComponent } from './inspecoes-abertas/inspecoes-abertas.component';
import { ArquivosInspecaoSaidaModule } from './arquivos-inspecao-saida/arquivos-inspecao-saida.module';
import { FormsModule } from '@angular/forms';
import { HistoricoInspecaoService } from './historico/historico-inspecao.service';
import { HistoricoInspecaoViewService } from './historico/historico-inspecao-view/historico-inspecao-view.service';
import { FiltrosInspecaoComponent } from './filtros-inspecao/filtros-inspecao.component';
import {
  FiltrosInspecaoModalComponent
} from './filtros-inspecao/filtros-inspecao-modal/filtros-inspecao-modal.component';

@NgModule({
  declarations: [
    QualidadeInspecaoSaidaComponent,
    OrdemProducaoViewComponent,
    InspecaoViewComponent,
    InspecaoDetailsComponent,
    AlterarDadosInspecaoModalComponent,
    FinalizarInspecaoModalComponent,
    InspecoesAbertasComponent,
    FiltrosInspecaoComponent,
    FiltrosInspecaoModalComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: QUALIDADE_INSPECAO_SAIDA_I18N_PT
      }
    }),
    VsLayoutModule,
    VsLabelModule,
    QualidadeInspecaoSaidaRoutingModule,
    VsGridModule,
    VsFormModule,
    VsInputModule,
    VsIconModule,
    TabsViewTemplateModule,
    VsButtonModule,
    VsDialogModule,
    VsSelectModule,
    VsCheckboxModule,
    VsDatepickerModule,
    ComponentsModule,
    VsTabGroupModule,
    RncLibModule,
    PersonLibStartupModule,
    FileProviderTagStartupModule,
    VsTextareaModule,
    HistoricoInspecaoModule,
    ArquivosInspecaoSaidaModule,
    FormsModule,
    VsChipListModule
  ],
  providers: [
    QualidadeInspecaoSaidaService,
    HistoricoInspecaoService,
    HistoricoInspecaoViewService
  ],
  exports: [
    QualidadeInspecaoSaidaComponent
  ]
})
export class QualidadeInspecaoSaidaModule { }
