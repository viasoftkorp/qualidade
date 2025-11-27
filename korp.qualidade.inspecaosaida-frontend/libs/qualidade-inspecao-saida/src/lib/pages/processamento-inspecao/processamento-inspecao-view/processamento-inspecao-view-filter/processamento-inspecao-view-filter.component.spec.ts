import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessamentoInspecaoViewFilterComponent } from './processamento-inspecao-view-filter.component';

describe('ProcessamentoInspecaoViewFilterComponent', () => {
  let component: ProcessamentoInspecaoViewFilterComponent;
  let fixture: ComponentFixture<ProcessamentoInspecaoViewFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProcessamentoInspecaoViewFilterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessamentoInspecaoViewFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
