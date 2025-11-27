import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NaturezaAutocompleteSelectComponent } from './natureza-autocomplete-select.component';

describe('NaturezaAutocompleteSelectComponent', () => {
  let component: NaturezaAutocompleteSelectComponent;
  let fixture: ComponentFixture<NaturezaAutocompleteSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NaturezaAutocompleteSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NaturezaAutocompleteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
