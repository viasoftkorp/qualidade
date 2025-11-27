import { TestBed } from '@angular/core/testing';

import { CausaService } from './causa.service';

describe('CausaService', () => {
  let service: CausaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CausaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
