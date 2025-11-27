import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule,
  VsDialogModule,
  VsGridModule,
  VsInputModule,
  VsLabelModule,
  VsSpinnerModule,
  VsTableModule,
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { NaturezaEditorModalComponent } from './natureza-editor-modal/natureza-editor-modal.component';
import { NATUREZA_I18N_EN } from './i18n/consts/natureza-i18n-en.const';
import { NATUREZA_I18N_PT } from './i18n/consts/natureza-i18n-pt.const';
import { NaturezaRoutingModule } from './natureza-routing.module';
import { NaturezaComponent } from './natureza.component';

@NgModule({
  declarations: [
    NaturezaComponent,
    NaturezaEditorModalComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: NATUREZA_I18N_PT,
        en: NATUREZA_I18N_EN
      }
    }),
    NaturezaRoutingModule,
    VsCommonModule,
    VsDialogModule,
    VsLabelModule,
    VsInputModule,
    VsButtonModule,
    VsTableModule,
    TabsViewTemplateModule,
    VsGridModule,
    VsSpinnerModule
  ]
})
export class NaturezaModule { }
