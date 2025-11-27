import { TestBed } from '@angular/core/testing';

import { HistoricoInspecaoService } from './historico-inspecao.service';

describe('HistoricoInspecaoService', () => {
  let service: HistoricoInspecaoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HistoricoInspecaoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
