import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddHandleRequestComponent } from './add-handle-request.component';

describe('AddHandleRequestComponent', () => {
  let component: AddHandleRequestComponent;
  let fixture: ComponentFixture<AddHandleRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddHandleRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddHandleRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
