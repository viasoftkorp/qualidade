import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoricoInspecaoViewFilterComponent } from './historico-inspecao-view-filter.component';

describe('HistoricoInspecaoViewFilterComponent', () => {
  let component: HistoricoInspecaoViewFilterComponent;
  let fixture: ComponentFixture<HistoricoInspecaoViewFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HistoricoInspecaoViewFilterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoricoInspecaoViewFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
