import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessamentoInspecaoViewComponent } from './processamento-inspecao-view.component';

describe('ProcessamentoInspecaoViewComponent', () => {
  let component: ProcessamentoInspecaoViewComponent;
  let fixture: ComponentFixture<ProcessamentoInspecaoViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProcessamentoInspecaoViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessamentoInspecaoViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
