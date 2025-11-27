import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessamentoInspecaoDetailsModalComponent } from './processamento-inspecao-details-modal.component';

describe('ProcessamentoInspecaoDetailsModalComponent', () => {
  let component: ProcessamentoInspecaoDetailsModalComponent;
  let fixture: ComponentFixture<ProcessamentoInspecaoDetailsModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProcessamentoInspecaoDetailsModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessamentoInspecaoDetailsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
