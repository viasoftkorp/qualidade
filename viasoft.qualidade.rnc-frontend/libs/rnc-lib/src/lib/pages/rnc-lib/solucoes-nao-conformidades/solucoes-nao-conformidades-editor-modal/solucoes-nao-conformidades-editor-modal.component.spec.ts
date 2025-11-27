/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { SolucoesNaoConformidadesEditorModalComponent } from './solucoes-nao-conformidades-editor-modal.component';

describe('SolucoesNaoConformidadesEditorComponent', () => {
  let component: SolucoesNaoConformidadesEditorModalComponent;
  let fixture: ComponentFixture<SolucoesNaoConformidadesEditorModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolucoesNaoConformidadesEditorModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolucoesNaoConformidadesEditorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
