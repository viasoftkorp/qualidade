/* eslint-disable no-use-before-define */

/* eslint-disable import/no-cycle */
/* eslint-disable max-classes-per-file */

import {
  Component, EventEmitter, forwardRef, Input, OnDestroy, Output
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

import { map } from 'rxjs/operators';

import { IPagedResultOutputDto, VsSubscriptionManager } from '@viasoft/common';
import {
  VsAutocompleteGetInput,
  VsAutocompleteGetNameFn,
  VsAutocompleteOutput,
  VsFormManipulator,
  VsGridGetInput
} from '@viasoft/components';

import { Observable } from 'rxjs';
import { RecursoAutocompleteSelectService } from './recurso-provider/recurso-autocomplete-select.service';

export class RecursoOutput {
  public id: string;
  public codigo: string;
  public descricao: string;
}

@Component({
  selector: 'qa-recurso-autocomplete-select',
  templateUrl: './recurso-autocomplete-select.component.html',
  styleUrls: ['./recurso-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RecursoAutocompleteSelectComponent),
      multi: true
    }
  ]
})
export class RecursoAutocompleteSelectComponent extends VsFormManipulator<string> implements OnDestroy {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Output() public loaded = new EventEmitter<RecursoOutput>();

  private subs: VsSubscriptionManager = new VsSubscriptionManager();

  constructor(
    private service: RecursoAutocompleteSelectService,
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }
  public getNames: VsAutocompleteGetNameFn<string> =
  (value) => this.service.get(value)
    .pipe(map((recurso:RecursoOutput) => {
      this.loaded.emit(recurso);
      return `${recurso.codigo} - ${recurso.descricao}`;
    }));

  public getAutocompleteItems =
  (input: VsAutocompleteGetInput): Observable<VsAutocompleteOutput<string>> => this.service.getList(
    {
      maxResultCount: input.maxDropSize,
      skipCount: input.skipCount,
      filter: input.valueToFilter,
      sorting: '',
      advancedFilter: ''
    } as VsGridGetInput
  ).pipe(
    map((pagedResult: IPagedResultOutputDto<RecursoOutput>) => ({
      totalCount: pagedResult.totalCount,
      items: pagedResult.items.map((recurso: RecursoOutput) => ({
        name: `${recurso.codigo} - ${recurso.descricao}`,
        value: recurso.id
      }))
    } as VsAutocompleteOutput<string>))
  )
}
