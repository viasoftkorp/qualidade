import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  VsButtonModule, VsCheckboxModule, VsDialogModule, VsGridModule, VsInputModule,
  VsLabelModule, VsSelectModule, VsTabGroupModule, VsTableModule
} from '@viasoft/components';
import { VsCommonModule } from '@viasoft/common';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { SolucaoRoutingModule } from './solucao-routing.module';
import { SolucaoComponent } from './solucao.component';
import { SOLUCAO_I18N_EN } from './i18n/consts/solucao-i18n-en.const';
import { SOLUCAO_I18N_PT } from './i18n/consts/solucao-i18n-pt.const';

@NgModule({
  declarations: [
    SolucaoComponent,
  ],
  imports: [
    CommonModule,
    VsCommonModule.forChild({
      translates: {
        pt: SOLUCAO_I18N_PT,
        en: SOLUCAO_I18N_EN
      }
    }),
    SolucaoRoutingModule,
    VsCommonModule,
    VsDialogModule,
    VsLabelModule,
    VsInputModule,
    VsButtonModule,
    VsTableModule,
    TabsViewTemplateModule,
    VsGridModule,
    VsCheckboxModule,
    VsSelectModule,
    VsTabGroupModule,
  ],
})
export class SolucaoModule { }
