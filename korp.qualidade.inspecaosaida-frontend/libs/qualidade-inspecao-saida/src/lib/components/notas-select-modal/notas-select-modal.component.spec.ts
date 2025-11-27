import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotasSelectModalComponent } from './notas-select-modal.component';

describe('NotasSelectModalComponent', () => {
  let component: NotasSelectModalComponent;
  let fixture: ComponentFixture<NotasSelectModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NotasSelectModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(NotasSelectModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
