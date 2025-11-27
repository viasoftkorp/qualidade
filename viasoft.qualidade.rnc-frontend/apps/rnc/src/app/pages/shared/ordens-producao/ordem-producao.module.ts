import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsAutocompleteModule } from '@viasoft/components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { OrdemProducaoService } from './ordem-producao.service';

@NgModule({
  imports: [
    CommonModule,
    VsAutocompleteModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    OrdemProducaoService
  ]
})
export class OrdemProducaoModule { }
