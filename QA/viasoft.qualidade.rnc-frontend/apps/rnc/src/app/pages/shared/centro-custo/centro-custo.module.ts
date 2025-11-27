import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VsAutocompleteModule } from '@viasoft/components';
import { CentroCustoService } from './centro-custo.service';
// eslint-disable-next-line max-len
import { CentroCustoAutocompleteChipsComponent } from './centro-custo-autocomplete-chips/centro-custo-autocomplete-chips.component';

@NgModule({
  declarations: [
    CentroCustoAutocompleteChipsComponent
  ],
  imports: [
    CommonModule,
    VsAutocompleteModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    CentroCustoAutocompleteChipsComponent
  ],
  providers: [
    CentroCustoService
  ]
})
export class CentroCustoModule { }
