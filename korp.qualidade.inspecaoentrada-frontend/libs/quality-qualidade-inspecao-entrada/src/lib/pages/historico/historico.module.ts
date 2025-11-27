import { DecimalPipe } from '@angular/common';
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
  VsSelectModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { ComponentsModule } from '../../components/components.module';
import { HistoricoService } from '../../services/historico.service';
import {
  HistoricoInspecaoDetailsModalComponent
} from './historico-inspecao-details-modal/historico-inspecao-details-modal.component';
import { HistoricoRoutingModule } from './historico-routing.module';
import { HistoricoComponent } from './historico.component';
import { HISTORICO_INSPECAO_I18N_PT } from './i18n/consts/historico-inspecao.component';
import {
  HistoricoInspecaoViewFilterComponent
} from './inspecao-historico-view/historico-inspecao-view-filter/historico-inspecao-view-filter.component';
import { HistoricoInspecaoViewService } from './inspecao-historico-view/historico-inspecao-view.service';
import { InspecaoHistoricoViewComponent } from './inspecao-historico-view/inspecao-historico-view.component';
import { RncLibModule } from '@viasoft/rnc-lib';
import { PersonLibStartupModule } from '@viasoft/person-lib';
import { FileProviderTagStartupModule } from '@viasoft/custom-file-provider-tag';

@NgModule({
  declarations: [
    HistoricoComponent,
    //InspecaoHistoricoViewComponent,
    HistoricoInspecaoViewFilterComponent,
    HistoricoInspecaoDetailsModalComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: { pt: HISTORICO_INSPECAO_I18N_PT }
    }),
    HistoricoRoutingModule,
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
    RncLibModule,
    PersonLibStartupModule,
    FileProviderTagStartupModule
  ],
  providers: [
    // HistoricoService,
    // HistoricoInspecaoViewService,
    // DecimalPipe
  ]
})
export class HistoricoModule { }
