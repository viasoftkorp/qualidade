import { TestBed } from '@angular/core/testing';

import { AcaoPreventivaService } from './acao-preventiva.service';

describe('AcaoPreventivaService', () => {
  let service: AcaoPreventivaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AcaoPreventivaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
