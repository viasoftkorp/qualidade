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
  VsSelectModule, VsTabGroupModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';

import { PROCESSAMENTO_INSPECAO_I18N_PT } from './i18n/consts/processamento-inspecao.component';

import { ProcessamentoInspecaoService } from './processamento-inspecao.service';
import { ProcessamentoInspecaoRoutingModule } from './processamento-inspecao-routing.module';
import { ProcessamentoInspecaoComponent } from './processamento-inspecao.component';
import { ProcessamentoInspecaoViewComponent } from './processamento-inspecao-view/processamento-inspecao-view.component';
import { ProcessamentoInspecaoViewFilterComponent } from './processamento-inspecao-view/processamento-inspecao-view-filter/processamento-inspecao-view-filter.component';
import { ComponentsModule } from '../../components/components.module';
import { ProcessamentoInspecaoGridComponent } from './processamento-inspecao-view/processamento-inspecao-grid/processamento-inspecao-grid.component';
import { ProcessamentoInspecaoDetailsModalComponent } from './processamento-inspecao-details-modal/processamento-inspecao-details-modal.component';
import { ProcessamentoInspecaoViewService } from './processamento-inspecao-view/processamento-inspecao-view.service';

@NgModule({
  declarations: [
    ProcessamentoInspecaoComponent,
    ProcessamentoInspecaoViewComponent,
    ProcessamentoInspecaoViewFilterComponent,
    ProcessamentoInspecaoGridComponent,
    ProcessamentoInspecaoDetailsModalComponent,
  ],
  imports: [
    VsCommonModule.forChild({
      translates: { pt: PROCESSAMENTO_INSPECAO_I18N_PT }
    }),
    ProcessamentoInspecaoRoutingModule,
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
    VsTabGroupModule
  ],
  providers: [
    ProcessamentoInspecaoService,
    ProcessamentoInspecaoViewService
  ]
})
export class ProcessamentoInspecaoModule { }
