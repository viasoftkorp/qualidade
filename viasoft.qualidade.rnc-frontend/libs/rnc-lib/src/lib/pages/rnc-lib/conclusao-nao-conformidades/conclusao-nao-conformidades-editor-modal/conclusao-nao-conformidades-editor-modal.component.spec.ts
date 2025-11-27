import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConclusaoNaoConformidadesEditorModalComponent } from './conclusao-nao-conformidades-editor-modal.component';

describe('ConclusaoNaoConformidadesEditorModalComponent', () => {
  let component: ConclusaoNaoConformidadesEditorModalComponent;
  let fixture: ComponentFixture<ConclusaoNaoConformidadesEditorModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConclusaoNaoConformidadesEditorModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConclusaoNaoConformidadesEditorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
