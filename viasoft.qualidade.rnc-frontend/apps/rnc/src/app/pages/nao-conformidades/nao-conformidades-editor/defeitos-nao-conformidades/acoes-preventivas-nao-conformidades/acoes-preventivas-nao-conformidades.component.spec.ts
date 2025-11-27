/* eslint-disable @typescript-eslint/no-unused-vars */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AcoesPreventivasNaoConformidadesComponent } from './acoes-preventivas-nao-conformidades.component';

describe('AcoesPreventivasNaoConformidadesComponent', () => {
  let component: AcoesPreventivasNaoConformidadesComponent;
  let fixture: ComponentFixture<AcoesPreventivasNaoConformidadesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcoesPreventivasNaoConformidadesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcoesPreventivasNaoConformidadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
