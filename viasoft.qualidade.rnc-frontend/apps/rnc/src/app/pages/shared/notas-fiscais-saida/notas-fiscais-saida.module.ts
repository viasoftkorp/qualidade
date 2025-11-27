import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VsAutocompleteModule } from '@viasoft/components';
// eslint-disable-next-line max-len
import { NotasFiscaisSaidaAutocompleteSelectComponent } from './notas-fiscais-saida-autocomplete-select/notas-fiscais-saida-autocomplete-select.component';

@NgModule({
  declarations: [
    NotasFiscaisSaidaAutocompleteSelectComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    VsAutocompleteModule
  ],
  exports: [
    NotasFiscaisSaidaAutocompleteSelectComponent
  ]
})
export class NotasFiscaisSaidaModule { }
