import { TestBed } from '@angular/core/testing';

import { LocalAutocompleteSelectService } from './produto-autocomplete-select.service';

describe('ProdutoAutocompleteSelectService', () => {
  let service: LocalAutocompleteSelectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LocalAutocompleteSelectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
