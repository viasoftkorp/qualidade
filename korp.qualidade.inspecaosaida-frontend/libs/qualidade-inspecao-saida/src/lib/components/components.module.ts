import { NgModule } from '@angular/core';

import { VsCommonModule } from '@viasoft/common';
import {VsAutocompleteModule, VsSearchModule} from '@viasoft/components';
import { VsSelectModalLegacyModule } from "@viasoft/components/select-modal-legacy";

import { ProdutoAutocompleteSelectComponent } from './produto-autocomplete-select/produto-autocomplete-select.component';
import { LocalAutocompleteSelectComponent } from './local-autocomplete-select/local-autocomplete-select.component';
import { UsuarioAutocompleteSelectComponent } from './usuario-autocomplete-select/usuario-autocomplete-select.component';
import { NotasSelectModalComponent } from "./notas-select-modal/notas-select-modal.component";

@NgModule({
  declarations: [
    ProdutoAutocompleteSelectComponent,
    LocalAutocompleteSelectComponent,
    UsuarioAutocompleteSelectComponent,
    NotasSelectModalComponent,
  ],
  exports: [
    ProdutoAutocompleteSelectComponent,
    LocalAutocompleteSelectComponent,
    UsuarioAutocompleteSelectComponent,
    NotasSelectModalComponent,
  ],
  imports: [
    VsCommonModule.forChild(),
    VsAutocompleteModule,
    VsSelectModalLegacyModule,
    VsSearchModule,
  ]
})
export class ComponentsModule {
}
