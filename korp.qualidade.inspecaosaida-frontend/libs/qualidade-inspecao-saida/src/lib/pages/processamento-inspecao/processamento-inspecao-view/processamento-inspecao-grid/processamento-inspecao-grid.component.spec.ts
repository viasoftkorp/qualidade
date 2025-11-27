import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessamentoInspecaoGridComponent } from './processamento-inspecao-grid.component';

describe('ProcessamentoInspecaoGridComponent', () => {
  let component: ProcessamentoInspecaoGridComponent;
  let fixture: ComponentFixture<ProcessamentoInspecaoGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProcessamentoInspecaoGridComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessamentoInspecaoGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
