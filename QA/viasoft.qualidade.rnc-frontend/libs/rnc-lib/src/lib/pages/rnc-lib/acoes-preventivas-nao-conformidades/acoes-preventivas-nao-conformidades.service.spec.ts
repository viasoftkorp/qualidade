import { TestBed } from '@angular/core/testing';

import { AcoesPreventivasNaoConformidadesService } from './acoes-preventivas-nao-conformidades.service';

describe('AcoesPreventivasNaoConformidadesService', () => {
  let service: AcoesPreventivasNaoConformidadesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AcoesPreventivasNaoConformidadesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
