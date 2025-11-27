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
import { UsuarioAutocompleteSelectService } from './usuario-autocomplete-select.service';
import {UsuarioOutput} from "./usuario-output.class";

@Component({
  selector: 'qa-usuario-autocomplete-select',
  templateUrl: './usuario-autocomplete-select.component.html',
  styleUrls: ['./usuario-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => UsuarioAutocompleteSelectComponent),
      multi: true
    },
    UsuarioAutocompleteSelectService
  ]
})
export class UsuarioAutocompleteSelectComponent extends VsFormManipulator<string> {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Input() public autoFocusOnLoad = false;
  @Input() public autoFocusFirstOption = true;
  @Input() public cleanable = true;
  @Output() public loaded = new EventEmitter<UsuarioOutput>();
  @Output() public cleared = new EventEmitter();
  @Output() public optionSelected = new EventEmitter<KeyValue<string, string>>();
  @Output() public closed = new EventEmitter<KeyValue<string, string>>();

  constructor(
    private usuarioAutocompleteSelectService: UsuarioAutocompleteSelectService,
  ) {
    super();
  }

  public getNames: VsAutocompleteGetNameFn<string> = (value) =>
    this.usuarioAutocompleteSelectService.get(value)
      .pipe(map((usuario: UsuarioOutput) => {
        this.loaded.emit(usuario);
        return usuario.login;
      }));

  public getAutocompleteItems = (input: VsAutocompleteGetInput): Observable<VsAutocompleteOutput<string>> =>
    this.usuarioAutocompleteSelectService.getList(input.valueToFilter, input.skipCount, input.maxDropSize)
      .pipe(map((pagedResult: IPagedResultOutputDto<UsuarioOutput>) => {
        if (!pagedResult.items || pagedResult.items.length === 0) {
          return {
            totalCount: 0,
            items: []
          }
        }

        return {
          totalCount: pagedResult.totalCount,
          items: pagedResult.items.map((usuario: UsuarioOutput) => ({
            name: usuario.login,
            value: usuario.id
          }))
        }
      }));
}
