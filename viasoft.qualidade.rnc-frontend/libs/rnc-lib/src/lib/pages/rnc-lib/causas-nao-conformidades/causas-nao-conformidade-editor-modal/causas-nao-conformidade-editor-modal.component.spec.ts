/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CausasNaoConformidadeEditorModalComponent } from './causas-nao-conformidade-editor-modal.component';

describe('CausasNaoConformidadeEditorModalComponent', () => {
  let component: CausasNaoConformidadeEditorModalComponent;
  let fixture: ComponentFixture<CausasNaoConformidadeEditorModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CausasNaoConformidadeEditorModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CausasNaoConformidadeEditorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
