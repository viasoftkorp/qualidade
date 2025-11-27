import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConclusaoNaoConformidadesComponent } from './conclusao-nao-conformidades.component';

describe('ConclusaoNaoConformidadesComponent', () => {
  let component: ConclusaoNaoConformidadesComponent;
  let fixture: ComponentFixture<ConclusaoNaoConformidadesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConclusaoNaoConformidadesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConclusaoNaoConformidadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
