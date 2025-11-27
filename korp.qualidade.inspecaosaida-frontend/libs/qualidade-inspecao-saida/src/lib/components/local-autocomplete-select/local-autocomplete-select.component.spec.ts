import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LocalAutocompleteSelectComponent } from './produto-autocomplete-select.component';

describe('LocalAutocompleteSelectComponent', () => {
  let component: LocalAutocompleteSelectComponent;
  let fixture: ComponentFixture<LocalAutocompleteSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LocalAutocompleteSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LocalAutocompleteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
