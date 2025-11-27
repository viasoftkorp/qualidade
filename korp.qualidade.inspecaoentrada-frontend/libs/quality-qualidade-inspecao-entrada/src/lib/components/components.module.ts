import { NgModule } from '@angular/core';

import { VsCommonModule } from '@viasoft/common';
import { VsAutocompleteModule } from '@viasoft/components';

import { ProdutoAutocompleteSelectComponent } from './produto-autocomplete-select/produto-autocomplete-select.component';
import { LocalAutocompleteSelectComponent } from './local-autocomplete-select/local-autocomplete-select.component';
import { UsuarioAutocompleteSelectComponent } from './usuario-autocomplete-select/usuario-autocomplete-select.component';

@NgModule({
  declarations: [
    ProdutoAutocompleteSelectComponent,
    LocalAutocompleteSelectComponent,
    UsuarioAutocompleteSelectComponent,
  ],
  exports: [
    ProdutoAutocompleteSelectComponent,
    LocalAutocompleteSelectComponent,
    UsuarioAutocompleteSelectComponent,
  ],
  imports: [
    VsCommonModule.forChild(),
    VsAutocompleteModule
  ]
})
export class ComponentsModule {
}
