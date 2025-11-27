import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsCommonModule } from '@viasoft/common';
import { VsSearchInputModule, VsSelectModalModule } from '@viasoft/components';
// eslint-disable-next-line max-len
import { EstoqueLocalSelectComponent } from './estoque-local-select/estoque-local-select.component';
import { ESTOQUE_LOCAL_I18N_EN } from './i18n/consts/estoque-local-i18n-en.const';
import { ESTOQUE_LOCAL_I18N_PT } from './i18n/consts/estoque-local-i18n-pt.const';

@NgModule({
  declarations: [
    EstoqueLocalSelectComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: ESTOQUE_LOCAL_I18N_PT,
        en: ESTOQUE_LOCAL_I18N_EN
      }
    }),
    CommonModule,
    VsSelectModalModule,
    VsSearchInputModule,

  ],
  exports: [
    EstoqueLocalSelectComponent
  ]
})
export class EstoqueLocalModule { }
