import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CausaComponent } from './causa.component';

describe('CausaComponent', () => {
  let component: CausaComponent;
  let fixture: ComponentFixture<CausaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CausaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CausaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
