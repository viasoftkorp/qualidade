import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DEFEITOS_PROXY_URL } from './tokens';
import { DefeitoAutocompleteSelectService } from './defeito-autocomplete-select.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    DefeitoAutocompleteSelectService, { provide: DEFEITOS_PROXY_URL, useValue: 'qualidade/rnc/gateway/defeitos' }]
})
export class DefeitoProviderModule { }
