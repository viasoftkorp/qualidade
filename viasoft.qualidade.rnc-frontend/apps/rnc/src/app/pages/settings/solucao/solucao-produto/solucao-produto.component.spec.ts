import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolucaoProdutoComponent } from './solucao-produto.component';

describe('SolucaoProdutoComponent', () => {
  let component: SolucaoProdutoComponent;
  let fixture: ComponentFixture<SolucaoProdutoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SolucaoProdutoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SolucaoProdutoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
