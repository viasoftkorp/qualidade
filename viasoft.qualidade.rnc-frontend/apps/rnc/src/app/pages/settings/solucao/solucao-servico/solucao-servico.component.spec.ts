import { ComponentFixture, TestBed } from '@angular/core/testing';

import {
  SolucaoServicoComponent
} from "@viasoft/rnc/app/pages/settings/solucao/solucao-servico/solucao-servico.component";

describe('SolucaoServicoComponent', () => {
  let component: SolucaoServicoComponent;
  let fixture: ComponentFixture<SolucaoServicoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SolucaoServicoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SolucaoServicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
