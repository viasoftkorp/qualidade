import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsuarioAutocompleteSelectComponent } from './usuario-autocomplete-select.component';

describe('UsuarioAutocompleteSelectComponent', () => {
  let component: UsuarioAutocompleteSelectComponent;
  let fixture: ComponentFixture<UsuarioAutocompleteSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsuarioAutocompleteSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UsuarioAutocompleteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
