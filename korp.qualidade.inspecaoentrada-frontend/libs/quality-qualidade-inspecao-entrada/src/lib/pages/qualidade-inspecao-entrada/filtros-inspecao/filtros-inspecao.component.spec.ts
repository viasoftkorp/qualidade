import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FiltrosInspecaoComponent } from './filtros-inspecao.component';

describe('FiltrosInspecaoComponent', () => {
  let component: FiltrosInspecaoComponent;
  let fixture: ComponentFixture<FiltrosInspecaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FiltrosInspecaoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FiltrosInspecaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
