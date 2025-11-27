import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import {
  VsGridModule,
  VsLabelModule,
  VsIconModule,
  VsButtonModule,
  VsDialogModule, VsFormModule, VsInputModule, VsCheckboxModule, VsRadioGroupModule, VsSearchModule, VsSelectModalModule, VsSearchInputModule,
  VsTabGroupModule
} from '@viasoft/components';
import { VsAutocompleteModule } from '@viasoft/components/autocomplete';
import { VsLayoutModule } from '@viasoft/components/layout';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { PRE_CADASTROS_I18N_PT } from './i18n/consts/pre-cadastros-i18n-pt.const';
import { PreCadastrosRoutingModule } from './pre-cadastros-routing.module';
import { PreCadastrosComponent } from './pre-cadastros.component';
import { MatTabsModule } from '@angular/material/tabs';
import { PreCadastroCttGenericComponent } from './pre-cadastro-ctt-generic/pre-cadastro-ctt-generic.component';

@NgModule({
  declarations: [
    PreCadastrosComponent,
    PreCadastroCttGenericComponent,
  ],
  imports: [
    VsCommonModule.forChild({ translates: { pt: PRE_CADASTROS_I18N_PT } }),
    PreCadastrosRoutingModule,
    VsLayoutModule,
    VsGridModule,
    VsLabelModule,
    VsIconModule,
    VsButtonModule,
    TabsViewTemplateModule,
    VsDialogModule,
    VsFormModule,
    VsInputModule,
    VsCheckboxModule,
    VsAutocompleteModule,
    VsRadioGroupModule,
    VsSearchModule,
    VsSelectModalModule,
    VsSearchInputModule,
    VsLayoutModule,
    VsIconModule,
    VsLabelModule,
    VsTabGroupModule,
    MatTabsModule,
  ],
})
export class PreCadastrosModule { }
