/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { SolucoesNaoConformidadesComponent } from './solucoes-nao-conformidades.component';

describe('SolucoesNaoConformidadesComponent', () => {
  let component: SolucoesNaoConformidadesComponent;
  let fixture: ComponentFixture<SolucoesNaoConformidadesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolucoesNaoConformidadesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolucoesNaoConformidadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
