import { TestBed } from '@angular/core/testing';

import { NaoConformidadesService } from './nao-conformidades.service';

describe('NaoConformidadesService', () => {
  let service: NaoConformidadesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NaoConformidadesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
