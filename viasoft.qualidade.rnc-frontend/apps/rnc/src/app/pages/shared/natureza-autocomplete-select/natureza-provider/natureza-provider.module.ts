import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NATUREZAS_PROXY_URL } from './tokens';
import { NaturezaAutocompleteSelectService } from './natureza-autocomplete-select.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    NaturezaAutocompleteSelectService, { provide: NATUREZAS_PROXY_URL, useValue: 'qualidade/rnc/gateway/naturezas' }]
})
export class NaturezaProviderModule { }
