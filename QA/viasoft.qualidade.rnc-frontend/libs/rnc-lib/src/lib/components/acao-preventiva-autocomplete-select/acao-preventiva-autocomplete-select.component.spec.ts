import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AcaoPreventivaAutocompleteSelectComponent } from './acao-preventiva-autocomplete-select.component';


describe('AcaoPreventivaAutocompleteSelectComponent', () => {
  let component: AcaoPreventivaAutocompleteSelectComponent;
  let fixture: ComponentFixture<AcaoPreventivaAutocompleteSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AcaoPreventivaAutocompleteSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AcaoPreventivaAutocompleteSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
