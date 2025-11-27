import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FiltrosInspecaoModalComponent } from './filtros-inspecao-modal.component';

describe('FiltrosInspecaoModalComponent', () => {
  let component: FiltrosInspecaoModalComponent;
  let fixture: ComponentFixture<FiltrosInspecaoModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FiltrosInspecaoModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FiltrosInspecaoModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
