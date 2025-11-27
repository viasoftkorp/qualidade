import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule,
  VsCheckboxModule, VsDatepickerModule,
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
import { OrdemProducaoViewFilterComponent } from './ordem-producao-view/ordem-producao-view-filter/ordem-producao-view-filter.component';
import { ComponentsModule } from '../../components/components.module';
import {
  HistoricoInspecaoViewComponent
} from "../historico-inspecao/historico-inspecao-view/historico-inspecao-view.component";
import { HistoricoInspecaoService } from "../historico-inspecao/historico-inspecao.service";
import {
  HistoricoInspecaoViewService
} from "../historico-inspecao/historico-inspecao-view/historico-inspecao-view.service";
import { PersonLibStartupModule } from '@viasoft/person-lib';
import { FileProviderTagStartupModule } from '@viasoft/custom-file-provider-tag';

@NgModule({
  declarations: [
    QualidadeInspecaoSaidaComponent,
    OrdemProducaoViewComponent,
    InspecaoViewComponent,
    InspecaoDetailsComponent,
    AlterarDadosInspecaoModalComponent,
    FinalizarInspecaoModalComponent,
    OrdemProducaoViewFilterComponent,
    HistoricoInspecaoViewComponent
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
    VsTextareaModule
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
