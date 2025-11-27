import { KeyValue } from '@angular/common';
import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { IPagedResultOutputDto } from '@viasoft/common';
import {
  VsAutocompleteGetInput,
  VsAutocompleteGetNameFn,
  VsAutocompleteOutput,
  VsFormManipulator
} from '@viasoft/components';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
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
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ProdutoAutocompleteSelectComponent),
      multi: true
    },
    ProdutoAutocompleteSelectService
  ]
})
export class ProdutoAutocompleteSelectComponent extends VsFormManipulator<string> {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Input() public autoFocusOnLoad = false;
  @Input() public autoFocusFirstOption = true;
  @Input() public cleanable = true;
  @Output() public loaded = new EventEmitter<ProdutoOutput>();
  @Output() public cleared = new EventEmitter();
  @Output() public optionSelected = new EventEmitter<KeyValue<string, string>>();
  @Output() public closed = new EventEmitter<KeyValue<string, string>>();

  constructor(
    private produtoAutocompleteSelectService: ProdutoAutocompleteSelectService,
  ) {
    super();
  }

  public getNames: VsAutocompleteGetNameFn<string> = (value) =>
    this.produtoAutocompleteSelectService.get(value)
      .pipe(map((produto: ProdutoOutput) => {
        this.loaded.emit(produto);
        return `${produto.codigo} ${produto.descricao}`;
      }));

  public getAutocompleteItems = (input: VsAutocompleteGetInput): Observable<VsAutocompleteOutput<string>> =>
    this.produtoAutocompleteSelectService.getList(input.valueToFilter, input.skipCount, input.maxDropSize)
      .pipe(map((pagedResult: IPagedResultOutputDto<ProdutoOutput>) => {
        if (!pagedResult.items || pagedResult.items.length === 0) {
          return {
            totalCount: 0,
            items: []
          }
        }

        return {
          totalCount: pagedResult.totalCount,
          items: pagedResult.items.map((produto: ProdutoOutput) => ({
            name: `${produto.codigo} ${produto.descricao}`,
            value: produto.codigo
          }))
        }
      }));
}
