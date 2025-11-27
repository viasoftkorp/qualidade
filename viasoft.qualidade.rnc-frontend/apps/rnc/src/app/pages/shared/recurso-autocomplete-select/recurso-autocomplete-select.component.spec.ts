import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecursoAutocompleteSelectComponent } from './recurso-autocomplete-select.component';

describe('RecursoAutocompleteSelectComponent', () => {
  let component: RecursoAutocompleteSelectComponent;
  let fixture: ComponentFixture<RecursoAutocompleteSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecursoAutocompleteSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecursoAutocompleteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
