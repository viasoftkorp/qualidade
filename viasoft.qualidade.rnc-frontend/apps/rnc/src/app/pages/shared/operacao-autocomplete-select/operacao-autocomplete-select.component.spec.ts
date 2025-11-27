import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OperacaoAutocompleteSelectComponent } from './operacao-autocomplete-select.component';

describe('OperacaoAutocompleteSelectComponent', () => {
  let component: OperacaoAutocompleteSelectComponent;
  let fixture: ComponentFixture<OperacaoAutocompleteSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OperacaoAutocompleteSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OperacaoAutocompleteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
