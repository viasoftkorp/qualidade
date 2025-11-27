import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsAutocompleteModule } from '@viasoft/components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PedidoVendaService } from './pedido-venda.service';
// eslint-disable-next-line max-len
import { PedidoVendaAutocompleteSelectComponent } from './pedido-venda-autocomplete-select/pedido-venda-autocomplete-select.component';

@NgModule({
  declarations: [
    PedidoVendaAutocompleteSelectComponent
  ],
  imports: [
    CommonModule,
    VsAutocompleteModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    PedidoVendaService
  ],
  exports: [
    PedidoVendaAutocompleteSelectComponent
  ]
})
export class PedidoVendaModule { }
