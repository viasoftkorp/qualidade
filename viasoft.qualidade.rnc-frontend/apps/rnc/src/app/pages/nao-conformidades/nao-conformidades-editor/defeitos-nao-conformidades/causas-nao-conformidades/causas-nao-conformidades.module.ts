import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule, VsGridModule, VsDialogModule, VsLabelModule,
  VsInputModule, VsFormModule, VsSelectModule, VsAutocompleteModule, VsTextareaModule, VsLayoutModule, VsSpinnerModule
} from '@viasoft/components';
import { TabsViewTemplateModule } from '@viasoft/view-template';

import { SharedModule } from '@viasoft/rnc/app/pages/shared/shared.module';
import { CausasNaoConformidadeEditorModalComponent }
  from './causas-nao-conformidade-editor-modal/causas-nao-conformidade-editor-modal.component';
import { CausasNaoConformidadesComponent } from './causas-nao-conformidades.component';
import { CAUSAS_NAO_CONFORMIDADE_I18N_EN } from './i18n/consts/causas-nao-conformidades-i18n-en.const';
import { CAUSAS_NAO_CONFORMIDADE_I18N_PT } from './i18n/consts/causas-nao-conformidades-i18n-pt.const';
import { CentroCustoModule } from '@viasoft/rnc/app/pages/shared/centro-custo/centro-custo.module';

@NgModule({
    imports: [
        CommonModule,
        VsCommonModule.forChild({
            translates: {
                pt: CAUSAS_NAO_CONFORMIDADE_I18N_PT,
                en: CAUSAS_NAO_CONFORMIDADE_I18N_EN
            }
        }),
        VsButtonModule,
        VsGridModule,
        TabsViewTemplateModule,
        VsDialogModule,
        VsLabelModule,
        VsInputModule,
        VsFormModule,
        VsCommonModule,
        VsSelectModule,
        VsAutocompleteModule,
        SharedModule,
        VsTextareaModule,
        VsLayoutModule,
        VsSpinnerModule,
        CentroCustoModule
    ],
  declarations: [CausasNaoConformidadesComponent, CausasNaoConformidadeEditorModalComponent],
  exports: [CausasNaoConformidadesComponent]
})
export class CausasNaoConformidadesModule { }
