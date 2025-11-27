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



import { UsuarioAutocompleteSelectService } from './usuario-autocomplete-select.service';

export class UsuarioOutput {
  public id: string;
  public login: string;
}

@Component({
  selector: 'qa-usuario-autocomplete-select',
  templateUrl: './usuario-autocomplete-select.component.html',
  styleUrls: ['./usuario-autocomplete-select.component.scss'],
  providers: [UsuarioAutocompleteSelectService]
})
export class UsuarioAutocompleteSelectComponent implements OnInit {
  @Input() public controlName: string;
  @Input() public placeholder: string;

  @Output() public usuarioAlterado: EventEmitter<UsuarioOutput> = new EventEmitter<UsuarioOutput>();

  public usuarioAutocompleteOptions: VsAutocompleteOptions;
  public usuarioAutocompleteGetInput: VsAutocompleteGetInput;

  private form: FormGroup;

  constructor(private formGroupDirective: FormGroupDirective, private service: UsuarioAutocompleteSelectService) {
  }

  ngOnInit(): void {
    this.form = this.formGroupDirective.form;

    this.buscarValorInicial();
    this.configuraUsuarioAutocomplete();
  }

  public usuarioChanged(event: KeyValue<string, any>): void {
    if (event) {
      const usuarioAlterado: UsuarioOutput = event.value;
      this.usuarioAlterado.emit(usuarioAlterado);
    }
  }

  private buscarValorInicial(): void {
    const idUsuario = this.form.get(this.controlName).value?.value;
    if (!idUsuario) {
      return;
    }

    this.service.get(idUsuario).subscribe((usuario: UsuarioOutput) => {
      this.form.get(this.controlName).setValue({
        key: usuario.login,
        value: idUsuario
      });
    });
  }

  private configuraUsuarioAutocomplete(): void {
    this.usuarioAutocompleteGetInput = { maxDropSize: 6 };
    this.usuarioAutocompleteOptions = new VsAutocompleteOptions();
    this.usuarioAutocompleteOptions.get = (i: VsAutocompleteGetInput) => {
      return this.service.getList(i.valueToFilter, i.skipCount, i.maxDropSize)
        .pipe(
          map((pagedResult: IPagedResultOutputDto<UsuarioOutput>) => {
            if (pagedResult && pagedResult.items) {
              return {
                items: pagedResult.items.map((usuario: UsuarioOutput) => ({
                  option: {
                    key: usuario.login,
                    value: usuario.id
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
