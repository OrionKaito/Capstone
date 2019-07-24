import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddManagePerGrComponent } from './add-manage-per-gr.component';

describe('AddManagePerGrComponent', () => {
  let component: AddManagePerGrComponent;
  let fixture: ComponentFixture<AddManagePerGrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddManagePerGrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddManagePerGrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
