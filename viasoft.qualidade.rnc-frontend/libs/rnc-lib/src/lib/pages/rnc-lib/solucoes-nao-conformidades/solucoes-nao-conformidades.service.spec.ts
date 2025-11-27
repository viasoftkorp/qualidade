import { TestBed } from '@angular/core/testing';

import { SolucoesNaoConformidadesService } from './solucoes-nao-conformidades.service';

describe('SolucoesNaoConformidadesService', () => {
  let service: SolucoesNaoConformidadesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SolucoesNaoConformidadesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
