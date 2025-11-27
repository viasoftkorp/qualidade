import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InspecoesAbertasComponent } from './inspecoes-abertas.component';

describe('InspecoesAbertasComponent', () => {
  let component: InspecoesAbertasComponent;
  let fixture: ComponentFixture<InspecoesAbertasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InspecoesAbertasComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InspecoesAbertasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
