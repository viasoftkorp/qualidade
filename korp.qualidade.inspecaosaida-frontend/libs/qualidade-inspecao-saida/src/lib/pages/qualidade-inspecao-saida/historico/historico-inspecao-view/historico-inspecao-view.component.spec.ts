import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoricoInspecaoViewComponent } from './historico-inspecao-view.component';

describe('HistoricoInspecaoViewComponent', () => {
  let component: HistoricoInspecaoViewComponent;
  let fixture: ComponentFixture<HistoricoInspecaoViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HistoricoInspecaoViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoricoInspecaoViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
