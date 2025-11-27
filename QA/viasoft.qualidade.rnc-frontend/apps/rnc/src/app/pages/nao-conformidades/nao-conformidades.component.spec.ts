import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NaoConformidadeComponent } from './nao-conformidades.component';

describe('NaoConformidadesComponent', () => {
  let component: NaoConformidadeComponent;
  let fixture: ComponentFixture<NaoConformidadeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NaoConformidadeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NaoConformidadeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
