import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessamentoInspecaoComponent } from './processamento-inspecao.component';

describe('ProcessamentoInspecaoComponent', () => {
  let component: ProcessamentoInspecaoComponent;
  let fixture: ComponentFixture<ProcessamentoInspecaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProcessamentoInspecaoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessamentoInspecaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
