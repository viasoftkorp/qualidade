import { TestBed } from '@angular/core/testing';

import { RecursoAutocompleteSelectService } from './recurso-autocomplete-select.service';

describe('RecursoAutocompleteSelectService', () => {
  let service: RecursoAutocompleteSelectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RecursoAutocompleteSelectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
