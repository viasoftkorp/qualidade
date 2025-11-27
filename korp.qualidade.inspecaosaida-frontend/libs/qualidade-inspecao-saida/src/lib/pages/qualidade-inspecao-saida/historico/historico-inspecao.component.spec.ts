import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoricoInspecaoComponent } from './historico-inspecao.component';

describe('HistoricoInspecaoComponent', () => {
  let component: HistoricoInspecaoComponent;
  let fixture: ComponentFixture<HistoricoInspecaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HistoricoInspecaoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoricoInspecaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
