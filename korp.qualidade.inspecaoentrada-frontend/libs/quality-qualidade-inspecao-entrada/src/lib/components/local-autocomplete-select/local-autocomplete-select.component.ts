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
import { LocalAutocompleteSelectService } from './local-autocomplete-select.service';
import { LocalOutput } from './local-output.class';

@Component({
  selector: 'qa-local-autocomplete-select',
  templateUrl: './local-autocomplete-select.component.html',
  styleUrls: ['./local-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => LocalAutocompleteSelectComponent),
      multi: true
    },
    LocalAutocompleteSelectService
  ]
})
export class LocalAutocompleteSelectComponent extends VsFormManipulator<string> {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Input() public autoFocusOnLoad = false;
  @Input() public autoFocusFirstOption = true;
  @Input() public cleanable = true;
  @Output() public loaded = new EventEmitter<LocalOutput>();
  @Output() public cleared = new EventEmitter();
  @Output() public optionSelected = new EventEmitter<KeyValue<string, string>>();
  @Output() public closed = new EventEmitter<KeyValue<string, string>>();

  constructor(
    private localAutocompleteSelectService: LocalAutocompleteSelectService,
  ) {
    super();
  }

  public getNames: VsAutocompleteGetNameFn<string> = (value) =>
    this.localAutocompleteSelectService.get(value)
      .pipe(map((local: LocalOutput) => {
        this.loaded.emit(local);
        return `${local.codigo} ${local.descricao}`;
      }));

  public getAutocompleteItems = (input: VsAutocompleteGetInput): Observable<VsAutocompleteOutput<string>> =>
    this.localAutocompleteSelectService.getList(input.valueToFilter, input.skipCount, input.maxDropSize)
      .pipe(map((pagedResult: IPagedResultOutputDto<LocalOutput>) => {
        if (!pagedResult.items || pagedResult.items.length === 0) {
          return {
            totalCount: 0,
            items: []
          }
        }

        return {
          totalCount: pagedResult.totalCount,
          items: pagedResult.items.map((local: LocalOutput) => ({
            name: `${local.codigo} ${local.descricao}`,
            value: local.codigo
          }))
        }
      }));
}
