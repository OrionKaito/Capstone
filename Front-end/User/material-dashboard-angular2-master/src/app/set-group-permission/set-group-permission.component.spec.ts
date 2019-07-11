import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SetGroupPermissionComponent } from './set-group-permission.component';

describe('SetGroupPermissionComponent', () => {
  let component: SetGroupPermissionComponent;
  let fixture: ComponentFixture<SetGroupPermissionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SetGroupPermissionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SetGroupPermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
