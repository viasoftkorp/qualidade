import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CAUSAS_PROXY_URL } from './tokens';
import { CausaAutocompleteSelectService } from './causa-autocomplete-select.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    CausaAutocompleteSelectService, { provide: CAUSAS_PROXY_URL, useValue: 'qualidade/rnc/core/causas' }]
})
export class CausaProviderModule { }
