import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolucaoEditorComponent } from './solucao-editor.component';

describe('SolucaoEditorComponent', () => {
  let component: SolucaoEditorComponent;
  let fixture: ComponentFixture<SolucaoEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SolucaoEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SolucaoEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
