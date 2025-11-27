import { TestBed } from '@angular/core/testing';

import { NaturezaAutocompleteSelectService } from './natureza-autocomplete-select.service';

describe('NaturezaAutocompleteSelectService', () => {
  let service: NaturezaAutocompleteSelectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NaturezaAutocompleteSelectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
