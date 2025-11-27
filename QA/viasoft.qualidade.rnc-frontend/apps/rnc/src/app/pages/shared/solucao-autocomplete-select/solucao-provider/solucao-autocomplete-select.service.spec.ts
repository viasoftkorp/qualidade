import { TestBed } from '@angular/core/testing';

import { SolucaoAutocompleteSelectService } from './solucao-autocomplete-select.service';

describe('SolucaoAutocompleteSelectService', () => {
  let service: SolucaoAutocompleteSelectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SolucaoAutocompleteSelectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
