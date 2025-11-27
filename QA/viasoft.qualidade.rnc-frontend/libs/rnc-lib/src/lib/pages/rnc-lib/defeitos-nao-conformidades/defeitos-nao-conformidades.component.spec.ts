/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DefeitosNaoConformidadesComponent } from './defeitos-nao-conformidades.component';

describe('DefeitosNaoConformidadesComponent', () => {
  let component: DefeitosNaoConformidadesComponent;
  let fixture: ComponentFixture<DefeitosNaoConformidadesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefeitosNaoConformidadesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefeitosNaoConformidadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
