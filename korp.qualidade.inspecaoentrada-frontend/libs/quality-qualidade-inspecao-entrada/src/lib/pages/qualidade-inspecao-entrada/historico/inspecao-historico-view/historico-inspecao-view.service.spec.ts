import { TestBed } from '@angular/core/testing';

import { HistoricoInspecaoViewService } from './historico-inspecao-view.service';

describe('HistoricoInspecaoViewService', () => {
  let service: HistoricoInspecaoViewService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HistoricoInspecaoViewService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
