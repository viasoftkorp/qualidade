import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NaoConformidadesFilesComponent } from './nao-conformidades-files.component';

describe('NaoConformidadesFilesComponent', () => {
  let component: NaoConformidadesFilesComponent;
  let fixture: ComponentFixture<NaoConformidadesFilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NaoConformidadesFilesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NaoConformidadesFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
