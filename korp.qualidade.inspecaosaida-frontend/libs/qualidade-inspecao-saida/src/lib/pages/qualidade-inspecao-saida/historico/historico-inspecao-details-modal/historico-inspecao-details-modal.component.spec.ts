import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoricoInspecaoDetailsModalComponent } from './historico-inspecao-details-modal.component';

describe('HistoricoInspecaoDetailsModalComponent', () => {
  let component: HistoricoInspecaoDetailsModalComponent;
  let fixture: ComponentFixture<HistoricoInspecaoDetailsModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HistoricoInspecaoDetailsModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoricoInspecaoDetailsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
