import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ACOES_PREVENTIVAS_PROXY_URL } from './tokens';
import { AcaoPreventivaAutocompleteSelectService } from './acao-preventiva-autocomplete-select.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    AcaoPreventivaAutocompleteSelectService,
    { provide: ACOES_PREVENTIVAS_PROXY_URL, useValue: 'qualidade/rnc/core/acoes-preventivas' }]
})
export class AcaoPreventivaProviderModule { }
