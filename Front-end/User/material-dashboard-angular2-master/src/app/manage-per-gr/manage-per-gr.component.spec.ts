import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagePerGrComponent } from './manage-per-gr.component';

describe('ManagePerGrComponent', () => {
  let component: ManagePerGrComponent;
  let fixture: ComponentFixture<ManagePerGrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagePerGrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagePerGrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
