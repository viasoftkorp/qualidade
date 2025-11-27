import { TestBed } from '@angular/core/testing';
import { AcaoPreventivaAutocompleteSelectService } from './acao-preventiva-autocomplete-select.service';


describe('AcaoPreventivaAutocompleteSelectService', () => {
  let service: AcaoPreventivaAutocompleteSelectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AcaoPreventivaAutocompleteSelectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
