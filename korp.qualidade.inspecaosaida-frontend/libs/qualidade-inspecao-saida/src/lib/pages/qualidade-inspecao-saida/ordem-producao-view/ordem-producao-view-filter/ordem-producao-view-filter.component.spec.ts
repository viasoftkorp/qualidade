import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdemProducaoViewFilterComponent } from './ordem-producao-view-filter.component';

describe('OrdemProducaoViewFilterComponent', () => {
  let component: OrdemProducaoViewFilterComponent;
  let fixture: ComponentFixture<OrdemProducaoViewFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrdemProducaoViewFilterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdemProducaoViewFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
