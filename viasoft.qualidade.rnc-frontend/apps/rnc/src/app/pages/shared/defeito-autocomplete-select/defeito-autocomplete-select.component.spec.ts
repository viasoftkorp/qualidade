import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DefeitoAutocompleteSelectComponent } from './defeito-autocomplete-select.component';

describe('DefeitoAutocompleteSelectComponent', () => {
  let component: DefeitoAutocompleteSelectComponent;
  let fixture: ComponentFixture<DefeitoAutocompleteSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DefeitoAutocompleteSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DefeitoAutocompleteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
