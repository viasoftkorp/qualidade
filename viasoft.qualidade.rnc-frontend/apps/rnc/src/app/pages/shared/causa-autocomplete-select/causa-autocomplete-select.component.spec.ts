import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CausaAutocompleteSelectComponent } from './causa-autocomplete-select.component';

describe('CausaAutocompleteSelectComponent', () => {
  let component: CausaAutocompleteSelectComponent;
  let fixture: ComponentFixture<CausaAutocompleteSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CausaAutocompleteSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CausaAutocompleteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
