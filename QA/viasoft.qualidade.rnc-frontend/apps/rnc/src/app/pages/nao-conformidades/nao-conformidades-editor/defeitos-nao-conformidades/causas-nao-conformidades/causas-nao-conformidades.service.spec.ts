import { TestBed } from '@angular/core/testing';

import { CausasNaoConformidadesService } from './causas-nao-conformidades.service';

describe('CausasNaoConformidadesService', () => {
  let service: CausasNaoConformidadesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CausasNaoConformidadesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
