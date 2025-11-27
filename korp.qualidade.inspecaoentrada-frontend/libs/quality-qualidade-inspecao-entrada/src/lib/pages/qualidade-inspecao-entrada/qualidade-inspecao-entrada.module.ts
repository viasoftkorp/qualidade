import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import {
  VsAutocompleteModule,
  VsButtonModule,
  VsCheckboxModule,
  VsDatepickerModule,
  VsDialogModule,
  VsFormModule,
  VsGridModule,
  VsIconModule,
  VsInputModule,
  VsLabelModule,
  VsLayoutModule,
  VsSelectModule, VsTabGroupModule, VsTextareaModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { QualidadeInspecaoEntradaService } from '../../services/qualidade-inspecao-entrada.service';
import { QUALIDADE_INSPECAO_ENTRADA_I18N_PT } from './i18/consts/qualidade-inspecao-entrada-i18-pt';
import {
  AlterarDadosInspecaoModalComponent
} from './inspecao-details/alterar-dados-inspecao-modal/alterar-dados-inspecao-modal.component';
import {
  FinalizarInspecaoModalComponent
} from './inspecao-details/finalizar-inspecao-modal/finalizar-inspecao-modal.component';
import { InspecaoDetailsComponent } from './inspecao-details/inspecao-details.component';
import { InspecaoViewComponent } from './inspecao-view/inspecao-view.component';
import { NotaFiscalViewComponent } from './notas-fiscais-view/nota-fiscal-view.component';
import { QualidadeInspecaoEntradaRoutingModule } from './qualidade-inspecao-entrada-routing.module';
import { QualidadeInspecaoEntradaComponent } from './qualidade-inspecao-entrada.component';
import {
  AlterarDadosFinalizacaoModalComponent
} from './inspecao-details/finalizar-inspecao-modal/alterar-dados-finalizacao-modal/alterar-dados-finalizacao-modal.component';
import {
  NotaFiscalViewFilterComponent
} from './notas-fiscais-view/nota-fiscal-view-filter/nota-fiscal-view-filter.component';
import { ComponentsModule } from '../../components/components.module';
import {InspecaoHistoricoViewComponent} from "../historico/inspecao-historico-view/inspecao-historico-view.component";
import {HistoricoService} from "../../services/historico.service";
import {HistoricoInspecaoViewService} from "../historico/inspecao-historico-view/historico-inspecao-view.service";
import {DecimalPipe} from "@angular/common";
import { RncLibModule } from '@viasoft/rnc-lib';
import { PersonLibStartupModule } from '@viasoft/person-lib';
import { FileProviderTagStartupModule } from '@viasoft/custom-file-provider-tag';

@NgModule({
  declarations: [
    QualidadeInspecaoEntradaComponent,
    NotaFiscalViewComponent,
    InspecaoViewComponent,
    InspecaoDetailsComponent,
    AlterarDadosInspecaoModalComponent,
    FinalizarInspecaoModalComponent,
    AlterarDadosFinalizacaoModalComponent,
    NotaFiscalViewFilterComponent,
    InspecaoHistoricoViewComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: QUALIDADE_INSPECAO_ENTRADA_I18N_PT,
      },
    }),
    QualidadeInspecaoEntradaRoutingModule,
    VsLayoutModule,
    VsLabelModule,
    VsGridModule,
    VsFormModule,
    VsInputModule,
    VsIconModule,
    TabsViewTemplateModule,
    VsButtonModule,
    VsDialogModule,
    VsSelectModule,
    VsCheckboxModule,
    VsAutocompleteModule,
    VsGridModule,
    VsDatepickerModule,
    ComponentsModule,
    VsTabGroupModule,
    VsTextareaModule,
    RncLibModule,
    PersonLibStartupModule,
    FileProviderTagStartupModule
  ],
  providers: [
    QualidadeInspecaoEntradaService,
    HistoricoService,
    HistoricoInspecaoViewService,
    DecimalPipe
  ],
  exports: [
    QualidadeInspecaoEntradaComponent
  ]
})
export class QualidadeInspecaoEntradaModule {}
