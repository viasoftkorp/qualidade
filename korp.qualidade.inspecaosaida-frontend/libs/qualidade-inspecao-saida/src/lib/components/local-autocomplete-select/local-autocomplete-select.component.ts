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

import { LocalAutocompleteSelectService } from './local-autocomplete-select.service';

export class LocalOutput {
  public id: string;
  public codigo: string;
  public descricao: string;
}

@Component({
  selector: 'qa-local-autocomplete-select',
  templateUrl: './local-autocomplete-select.component.html',
  styleUrls: ['./local-autocomplete-select.component.scss'],
  providers: [LocalAutocompleteSelectService]
})
export class LocalAutocompleteSelectComponent implements OnInit {
  @Input() public controlName: string;
  @Input() public placeholder: string;

  @Output() public localAlterado: EventEmitter<LocalOutput> = new EventEmitter<LocalOutput>();

  public localAutocompleteOptions: VsAutocompleteOptions;
  public localAutocompleteGetInput: VsAutocompleteGetInput;

  private form: FormGroup;

  constructor(private formGroupDirective: FormGroupDirective, private service: LocalAutocompleteSelectService) {
  }

  ngOnInit(): void {
    this.form = this.formGroupDirective.form;

    this.buscarValorInicial();
    this.configuraLocalAutocomplete();
  }

  public localChanged(event: KeyValue<string, any>): void {
    if (event) {
      const localAlterado: LocalOutput = event.value;
      this.localAlterado.emit(localAlterado);
    }
  }

  private buscarValorInicial(): void {
    const codigoLocal = this.form.get(this.controlName).value?.value;
    if (!codigoLocal) {
      return;
    }

    this.service.get(codigoLocal).subscribe((local: LocalOutput) => {
      this.form.get(this.controlName).setValue({
        key: `${local.codigo} - ${local.descricao}`,
        value: local.codigo
      });
    });
  }

  private configuraLocalAutocomplete(): void {
    this.localAutocompleteGetInput = { maxDropSize: 6 };
    this.localAutocompleteOptions = new VsAutocompleteOptions();
    this.localAutocompleteOptions.get = (i: VsAutocompleteGetInput) => {
      return this.service.getList(i.valueToFilter, i.skipCount, i.maxDropSize)
        .pipe(
          map((pagedResult: IPagedResultOutputDto<LocalOutput>) => {
            if (pagedResult && pagedResult.items) {
              return {
                items: pagedResult.items.map((local: LocalOutput) => ({
                  option: {
                    key: `${local.codigo} - ${local.descricao}`,
                    value: local.codigo
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
