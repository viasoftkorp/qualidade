import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule,
  VsDialogModule,
  VsGridModule,
  VsInputModule,
  VsLabelModule,
  VsSpinnerModule,
  VsTableModule, VsTextareaModule,
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { CausaEditorModalComponent } from './causa-editor-modal/causa-editor-modal.component';
import { CAUSA_I18N_EN } from './i18n/consts/causa-i18n-en.const';
import { CAUSA_I18N_PT } from './i18n/consts/causa-i18n-pt.const';
import { CausaRoutingModule } from './causa-routing.module';
import { CausaComponent } from './causa.component';

@NgModule({
  declarations: [
    CausaComponent,
    CausaEditorModalComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: CAUSA_I18N_PT,
        en: CAUSA_I18N_EN
      }
    }),
    CausaRoutingModule,
    VsCommonModule,
    VsDialogModule,
    VsLabelModule,
    VsInputModule,
    VsButtonModule,
    VsTableModule,
    TabsViewTemplateModule,
    VsGridModule,
    VsTextareaModule,
    VsSpinnerModule
  ]
})
export class CausaModule { }
