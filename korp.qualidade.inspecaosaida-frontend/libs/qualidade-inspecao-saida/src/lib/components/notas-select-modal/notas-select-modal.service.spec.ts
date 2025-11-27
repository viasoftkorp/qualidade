import { TestBed } from '@angular/core/testing';

import { NotasSelectModalService } from './notas-select-modal.service';

describe('NotasSelectModalService', () => {
  let service: NotasSelectModalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NotasSelectModalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
