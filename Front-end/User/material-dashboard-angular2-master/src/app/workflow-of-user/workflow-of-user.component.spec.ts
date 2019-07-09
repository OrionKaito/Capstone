import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowOfUserComponent } from './workflow-of-user.component';

describe('WorkflowOfUserComponent', () => {
  let component: WorkflowOfUserComponent;
  let fixture: ComponentFixture<WorkflowOfUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowOfUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowOfUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
