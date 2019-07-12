import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAccountShapeComponent } from './edit-account-shape.component';

describe('EditAccountShapeComponent', () => {
  let component: EditAccountShapeComponent;
  let fixture: ComponentFixture<EditAccountShapeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditAccountShapeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditAccountShapeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
