import { TestBed } from '@angular/core/testing';

import { DefeitoAutocompleteSelectService } from './defeito-autocomplete-select.service';

describe('DefeitoAutocompleteSelectService', () => {
  let service: DefeitoAutocompleteSelectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DefeitoAutocompleteSelectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
