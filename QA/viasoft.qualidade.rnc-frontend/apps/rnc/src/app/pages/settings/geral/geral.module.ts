import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule,
  VsCheckboxModule,
  VsFormModule,
  VsIconModule,
  VsLabelModule,
  VsLayoutModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { GeralComponent } from './geral.component';
import { GeralRoutingModule } from './geral-routing.module';
import { GERAL_I18N_EN } from './i18n/consts/geral-i18n.en.consts';
import { GERAL_I18N_PT } from './i18n/consts/geral-i18n.pt.consts';

@NgModule({
  declarations: [
    GeralComponent
  ],
  imports: [
    CommonModule,
    GeralRoutingModule,
    CommonModule,
    VsCommonModule.forChild({
      translates: {
        pt: GERAL_I18N_PT,
        en: GERAL_I18N_EN
      }
    }),
    VsCommonModule,
    VsButtonModule,
    VsCheckboxModule,
    TabsViewTemplateModule,
    VsIconModule,
    VsLayoutModule,
    VsLabelModule,
    VsFormModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class GeralModule { }
