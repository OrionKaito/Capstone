import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageErrRequestComponent } from './manage-err-request.component';

describe('ManageErrRequestComponent', () => {
  let component: ManageErrRequestComponent;
  let fixture: ComponentFixture<ManageErrRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageErrRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageErrRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
