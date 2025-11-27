/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DefeitosNaoConformidadesEditorModalComponent } from './defeitos-nao-conformidades-editor-modal.component';

describe('DefeitosNaoConformidadesEditorModalComponent', () => {
  let component: DefeitosNaoConformidadesEditorModalComponent;
  let fixture: ComponentFixture<DefeitosNaoConformidadesEditorModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefeitosNaoConformidadesEditorModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefeitosNaoConformidadesEditorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
