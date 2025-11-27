import { TestBed } from '@angular/core/testing';

import { ProcessamentoInspecaoViewService } from './processamento-inspecao-view.service';

describe('ProcessamentoInspecaoViewService', () => {
  let service: ProcessamentoInspecaoViewService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProcessamentoInspecaoViewService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
