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
  VsSelectModule,
  VsTabGroupModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { HISTORICO_INSPECAO_I18N_PT } from './i18n/consts/historico-inspecao.component';
import { HistoricoInspecaoRoutingModule } from './historico-inspecao-routing.module';
import { HistoricoInspecaoViewComponent } from './historico-inspecao-view/historico-inspecao-view.component';
import { ComponentsModule } from '../../../components/components.module';
import { HistoricoInspecaoDetailsModalComponent } from './historico-inspecao-details-modal/historico-inspecao-details-modal.component';
import { HistoricoInspecaoViewService } from './historico-inspecao-view/historico-inspecao-view.service';
import { RncLibModule } from '@viasoft/rnc-lib';
import { PersonLibStartupModule } from '@viasoft/person-lib';
import { FileProviderTagStartupModule } from '@viasoft/custom-file-provider-tag';
import { ArquivosInspecaoSaidaModule } from '../arquivos-inspecao-saida/arquivos-inspecao-saida.module';
import { HistoricoInspecaoService } from './historico-inspecao.service';

@NgModule({
  declarations: [
    HistoricoInspecaoViewComponent,
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
    FileProviderTagStartupModule,
    VsTabGroupModule,
    ArquivosInspecaoSaidaModule
  ],
  exports: [
    HistoricoInspecaoViewComponent,
  ],
  providers: [
    HistoricoInspecaoService,
    HistoricoInspecaoViewService
  ]
})
export class HistoricoInspecaoModule { }
