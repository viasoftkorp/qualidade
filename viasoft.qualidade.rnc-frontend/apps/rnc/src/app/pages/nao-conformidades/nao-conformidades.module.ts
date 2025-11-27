import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule, VsGridModule, VsLabelModule, VsInputModule,
  VsCheckboxModule, VsFormModule, VsLayoutModule, VsTextareaModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { HttpClientModule } from '@angular/common/http';
import { PersonAutocompleteSingleModule } from '@viasoft/person-lib';
import { VsFileProviderModule } from '@viasoft/file-provider';
import { NaoConformidadesRoutingModule } from './nao-conformidades-routing.module';
import { NaoConformidadeComponent } from './nao-conformidades.component';
import { NaoConformidadesEditorModule } from './nao-conformidades-editor/nao-conformidades-editor.module';
import { SharedModule } from '../shared/shared.module';
import { NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/nao-conformidades-i18n-pt.const';
import { NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/nao-conformidades-i18n-en.const';

@NgModule({
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: NAO_CONFORMIDADE_I18N_PT,
        en: NAO_CONFORMIDADE_I18N_EN
      }
    }),
    CommonModule,
    HttpClientModule,
    NaoConformidadesRoutingModule,
    VsButtonModule,
    TabsViewTemplateModule,
    VsGridModule,
    VsLabelModule,
    VsInputModule,
    VsCheckboxModule,
    MatTabsModule,
    VsFormModule,
    VsLayoutModule,
    VsCommonModule,
    VsTextareaModule,
    NaoConformidadesEditorModule,
    PersonAutocompleteSingleModule,
    SharedModule,
    VsFileProviderModule
  ],
  declarations: [NaoConformidadeComponent],
})
export class NaoConformidadesModule { }
