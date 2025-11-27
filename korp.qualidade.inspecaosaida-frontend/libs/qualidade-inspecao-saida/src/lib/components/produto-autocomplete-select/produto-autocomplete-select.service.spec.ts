import { TestBed } from '@angular/core/testing';

import { ProdutoAutocompleteSelectService } from './produto-autocomplete-select.service';

describe('ProdutoAutocompleteSelectService', () => {
  let service: ProdutoAutocompleteSelectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProdutoAutocompleteSelectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
