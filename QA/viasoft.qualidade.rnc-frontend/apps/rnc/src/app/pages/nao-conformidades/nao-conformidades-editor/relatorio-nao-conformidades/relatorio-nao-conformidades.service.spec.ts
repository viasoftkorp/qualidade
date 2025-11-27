import { TestBed } from '@angular/core/testing';

import { RelatorioNaoConformidadesService } from './relatorio-nao-conformidades.service';

describe('RelatorioNaoConformidadesService', () => {
  let service: RelatorioNaoConformidadesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RelatorioNaoConformidadesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
