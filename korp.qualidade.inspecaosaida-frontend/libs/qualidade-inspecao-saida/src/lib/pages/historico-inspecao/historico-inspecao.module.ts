import { NgModule } from '@angular/core';

import { VsCommonModule } from '@viasoft/common';
import {
  VsAutocompleteModule,
  VsButtonModule,
  VsDatepickerModule,
  VsDialogModule,
  VsFormModule,
  VsGridModule,
  VsIconModule,
  VsInputModule,
  VsLabelModule,
  VsSelectModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';

import { HISTORICO_INSPECAO_I18N_PT } from './i18n/consts/historico-inspecao.component';

import { HistoricoInspecaoService } from './historico-inspecao.service';
import { HistoricoInspecaoRoutingModule } from './historico-inspecao-routing.module';
import { HistoricoInspecaoComponent } from './historico-inspecao.component';
import { HistoricoInspecaoViewComponent } from './historico-inspecao-view/historico-inspecao-view.component';
import { HistoricoInspecaoViewFilterComponent } from './historico-inspecao-view/historico-inspecao-view-filter/historico-inspecao-view-filter.component';
import { ComponentsModule } from '../../components/components.module';
import { HistoricoInspecaoDetailsModalComponent } from './historico-inspecao-details-modal/historico-inspecao-details-modal.component';
import { HistoricoInspecaoViewService } from './historico-inspecao-view/historico-inspecao-view.service';
import { RncLibModule } from '@viasoft/rnc-lib';
import { PersonLibStartupModule } from '@viasoft/person-lib';
import { FileProviderTagStartupModule } from '@viasoft/custom-file-provider-tag';

@NgModule({
  declarations: [
    HistoricoInspecaoComponent,
   // HistoricoInspecaoViewComponent,
    HistoricoInspecaoViewFilterComponent,
    HistoricoInspecaoDetailsModalComponent,
  ],
  imports: [
    VsCommonModule.forChild({
      translates: { pt: HISTORICO_INSPECAO_I18N_PT }
    }),
    HistoricoInspecaoRoutingModule,
    VsButtonModule,
    VsGridModule,
    TabsViewTemplateModule,
    VsDialogModule,
    VsLabelModule,
    VsIconModule,
    VsSelectModule,
    VsFormModule,
    VsInputModule,
    VsDatepickerModule,
    VsAutocompleteModule,
    ComponentsModule,
    RncLibModule,
    PersonLibStartupModule,
    FileProviderTagStartupModule
  ],
  providers: [
    HistoricoInspecaoService,
    HistoricoInspecaoViewService
  ]
})
export class HistoricoInspecaoModule { }
