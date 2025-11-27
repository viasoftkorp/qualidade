import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SOLUCOES_PROXY_URL } from './tokens';
import { SolucaoAutocompleteSelectService } from './solucao-autocomplete-select.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    SolucaoAutocompleteSelectService, { provide: SOLUCOES_PROXY_URL, useValue: 'qualidade/rnc/gateway/solucoes' }]
})
export class SolucaoProviderModule { }
