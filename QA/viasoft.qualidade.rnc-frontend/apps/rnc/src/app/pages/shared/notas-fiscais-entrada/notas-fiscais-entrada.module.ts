import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { VsAutocompleteModule } from '@viasoft/components';
// eslint-disable-next-line max-len
import { NotasFiscaisEntradaAutocompleteSelectComponent } from './notas-fiscais-entrada-autocomplete-select/notas-fiscais-entrada-autocomplete-select.component';

@NgModule({
  declarations: [
    NotasFiscaisEntradaAutocompleteSelectComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    VsAutocompleteModule
  ],
  exports: [
    NotasFiscaisEntradaAutocompleteSelectComponent
  ]
})
export class NotasFiscaisEntradaModule { }
