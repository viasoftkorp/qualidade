import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProdutoAutocompleteSelectComponent } from './produto-autocomplete-select.component';

describe('ProdutoAutocompleteSelectComponent', () => {
  let component: ProdutoAutocompleteSelectComponent;
  let fixture: ComponentFixture<ProdutoAutocompleteSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProdutoAutocompleteSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProdutoAutocompleteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
