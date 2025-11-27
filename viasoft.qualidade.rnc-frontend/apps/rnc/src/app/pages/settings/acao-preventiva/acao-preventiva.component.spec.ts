import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AcaoPreventivaComponent } from './acao-preventiva.component';

describe('AcaoPreventivaComponent', () => {
  let component: AcaoPreventivaComponent;
  let fixture: ComponentFixture<AcaoPreventivaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AcaoPreventivaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AcaoPreventivaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
