/* eslint-disable @typescript-eslint/no-unused-vars */

import { TestBed, async, inject } from '@angular/core/testing';
import { DefeitosNaoConformidadesService } from './defeitos-nao-conformidades.service';

describe('Service: DefeitosNaoConformidades', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DefeitosNaoConformidadesService]
    });
  });

  it('should ...', inject([DefeitosNaoConformidadesService], (service: DefeitosNaoConformidadesService) => {
    expect(service).toBeTruthy();
  }));
});
