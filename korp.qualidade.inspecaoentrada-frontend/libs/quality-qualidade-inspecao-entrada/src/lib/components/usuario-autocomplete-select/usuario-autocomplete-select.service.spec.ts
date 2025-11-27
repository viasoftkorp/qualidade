import { TestBed } from '@angular/core/testing';

import { UsuarioAutocompleteSelectService } from './usuario-autocomplete-select.service';

describe('UsuarioAutocompleteSelectService', () => {
  let service: UsuarioAutocompleteSelectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UsuarioAutocompleteSelectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
