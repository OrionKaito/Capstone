import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNewDynamicFormComponent } from './add-new-dynamic-form.component';

describe('AddNewDynamicFormComponent', () => {
  let component: AddNewDynamicFormComponent;
  let fixture: ComponentFixture<AddNewDynamicFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddNewDynamicFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddNewDynamicFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
