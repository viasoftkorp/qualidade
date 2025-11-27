import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { KeyValue } from '@angular/common';
import { FormGroup, FormGroupDirective } from '@angular/forms';

import { map } from 'rxjs/operators';

import { IPagedResultOutputDto } from '@viasoft/common';
import {
  VsAutocompleteGetInput,
  VsAutocompleteOptions,
  VsAutocompleteValue
} from '@viasoft/components';

import {
  VsLegacyAutocompleteOutput
} from '@viasoft/components/autocomplete';

import { ProdutoAutocompleteSelectService } from './produto-autocomplete-select.service';

export class ProdutoOutput {
  public id: string;
  public codigo: string;
  public descricao: string;
}

@Component({
  selector: 'qa-produto-autocomplete-select',
  templateUrl: './produto-autocomplete-select.component.html',
  styleUrls: ['./produto-autocomplete-select.component.scss'],
  providers: [ProdutoAutocompleteSelectService]
})
export class ProdutoAutocompleteSelectComponent implements OnInit {
  @Input() public controlName: string;
  @Input() public placeholder: string;

  @Output() public produtoAlterado: EventEmitter<ProdutoOutput> = new EventEmitter<ProdutoOutput>();

  public produtoAutocompleteOptions: VsAutocompleteOptions;
  public produtoAutocompleteGetInput: VsAutocompleteGetInput;

  private form: FormGroup;

  constructor(private formGroupDirective: FormGroupDirective, private service: ProdutoAutocompleteSelectService) {
  }

  ngOnInit(): void {
    this.form = this.formGroupDirective.form;

    this.buscarValorInicial();
    this.configuraProdutoAutocomplete();
  }

  public produtoChanged(event: KeyValue<string, any>): void {
    if (event) {
      const produtoAlterado: ProdutoOutput = event.value;
      this.produtoAlterado.emit(produtoAlterado);
    }
  }

  private buscarValorInicial(): void {
    const codigoProduto = this.form.get(this.controlName).value?.value;
    if (!codigoProduto) {
      return;
    }

    this.service.get(codigoProduto).subscribe((produto: ProdutoOutput) => {
      this.form.get(this.controlName).setValue({
        key: `${produto.codigo} - ${produto.descricao}`,
        value: produto.codigo
      });
    });
  }

  private configuraProdutoAutocomplete(): void {
    this.produtoAutocompleteGetInput = { maxDropSize: 6 };
    this.produtoAutocompleteOptions = new VsAutocompleteOptions();
    this.produtoAutocompleteOptions.get = (i: VsAutocompleteGetInput) => {
      return this.service.getList(i.valueToFilter, i.skipCount, i.maxDropSize)
        .pipe(
          map((pagedResult: IPagedResultOutputDto<ProdutoOutput>) => {
            if (pagedResult && pagedResult.items) {
              return {
                items: pagedResult.items.map((produto: ProdutoOutput) => ({
                  option: {
                    key: `${produto.codigo} - ${produto.descricao}`,
                    value: produto.codigo
                  }
                }) as VsAutocompleteValue),
                totalCount: pagedResult.totalCount
              } as VsLegacyAutocompleteOutput
            }

            return {
              items: [],
              totalCount: 0
            } as VsLegacyAutocompleteOutput
          })
        );
    };
  }
}
