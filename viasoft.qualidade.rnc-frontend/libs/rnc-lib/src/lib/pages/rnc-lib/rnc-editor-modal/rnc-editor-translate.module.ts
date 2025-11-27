import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import { NAO_CONFORMIDADE_I18N_PT } from '../i18n/consts/nao-conformidades-i18n-pt.const';
import { NAO_CONFORMIDADE_I18N_EN } from '../i18n/consts/nao-conformidades-i18n-en.const';


@NgModule({
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt: NAO_CONFORMIDADE_I18N_PT,
        en: NAO_CONFORMIDADE_I18N_EN
      }
    })
  ]
})
export class RncEditorTranslateModule {
}
