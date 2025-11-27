import { TestBed } from '@angular/core/testing';

import { CausaAutocompleteSelectService } from './causa-autocomplete-select.service';

describe('CausaAutocompleteSelectService', () => {
  let service: CausaAutocompleteSelectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CausaAutocompleteSelectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
