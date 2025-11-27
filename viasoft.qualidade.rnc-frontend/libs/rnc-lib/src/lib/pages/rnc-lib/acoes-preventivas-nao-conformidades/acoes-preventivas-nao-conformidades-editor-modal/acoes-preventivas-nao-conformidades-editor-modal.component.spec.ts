import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AcoesPreventivasNaoConformidadesEditorModalComponent } from './acoes-preventivas-nao-conformidades-editor-modal.component';

describe('AcoesPreventivasNaoConformidadesEditorModalComponent', () => {
  let component: AcoesPreventivasNaoConformidadesEditorModalComponent;
  let fixture: ComponentFixture<AcoesPreventivasNaoConformidadesEditorModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AcoesPreventivasNaoConformidadesEditorModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AcoesPreventivasNaoConformidadesEditorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
