import { TestBed } from '@angular/core/testing';

import { ProcessamentoInspecaoService } from './processamento-inspecao.service';

describe('ProcessamentoInspecaoService', () => {
  let service: ProcessamentoInspecaoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProcessamentoInspecaoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
