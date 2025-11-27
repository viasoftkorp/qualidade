import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolucaoAutocompleteSelectComponent } from './solucao-autocomplete-select.component';

describe('SolucaoAutocompleteSelectComponent', () => {
  let component: SolucaoAutocompleteSelectComponent;
  let fixture: ComponentFixture<SolucaoAutocompleteSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SolucaoAutocompleteSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SolucaoAutocompleteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
