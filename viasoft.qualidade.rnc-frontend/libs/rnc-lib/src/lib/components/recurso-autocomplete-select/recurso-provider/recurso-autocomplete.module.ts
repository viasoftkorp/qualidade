import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecursoAutocompleteSelectService } from './recurso-autocomplete-select.service';
import { RECURSOS_PROXY_URL } from './tokens';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    RecursoAutocompleteSelectService, { provide: RECURSOS_PROXY_URL, useValue: 'qualidade/rnc/core/recursos' }]
})
export class RecursoProviderModule { }
