import { VsCommonModule } from '@viasoft/common';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  VsDialogModule, VsInputModule, VsLabelModule, VsButtonModule, VsTableModule, VsGridModule, VsTabGroupModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { SETTINGS_I18N_EN } from './i18n/consts/settings-i18n-en.const';
import { SETTINGS_I18N_PT } from './i18n/consts/settings-i18n-pt.const';
import { SettingsComponent } from './settings.component';
import { SettingsRoutingModule } from './settings-routing.module';

@NgModule({
  declarations: [SettingsComponent,
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: SETTINGS_I18N_PT,
        en: SETTINGS_I18N_EN
      }
    }),
    CommonModule,
    SettingsRoutingModule,
    VsCommonModule,
    VsDialogModule,
    VsLabelModule,
    VsInputModule,
    VsButtonModule,
    VsTableModule,
    TabsViewTemplateModule,
    VsGridModule,
    VsTabGroupModule

  ]
})
export class SettingsModule { }
