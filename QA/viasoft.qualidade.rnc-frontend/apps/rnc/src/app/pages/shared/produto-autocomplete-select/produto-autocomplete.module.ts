import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PRODUTOS_PROXY_URL } from './produto-provider/tokens';
import { ProdutoAutocompleteSelectService } from './produto-provider/produto-autocomplete-select.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    ProdutoAutocompleteSelectService, { provide: PRODUTOS_PROXY_URL, useValue: 'qualidade/rnc/gateway/produtos' }]
})
export class ProdutoProviderModule { }
