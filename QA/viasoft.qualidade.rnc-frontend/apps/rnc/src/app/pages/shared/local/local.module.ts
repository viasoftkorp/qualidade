import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VsAutocompleteModule } from '@viasoft/components';
import { LocalService } from './local.service';
import { LocaisAutocompleteSelectComponent } from './locais-autocomplete-select/locais-autocomplete-select.component';

@NgModule({
  declarations: [LocaisAutocompleteSelectComponent],
  imports: [
    CommonModule,
    VsAutocompleteModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    LocalService
  ],
  exports: [
    LocaisAutocompleteSelectComponent
  ]
})
export class LocalModule { }
