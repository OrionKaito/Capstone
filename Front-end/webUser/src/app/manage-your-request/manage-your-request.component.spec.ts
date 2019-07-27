import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageYourRequestComponent } from './manage-your-request.component';

describe('ManageYourRequestComponent', () => {
  let component: ManageYourRequestComponent;
  let fixture: ComponentFixture<ManageYourRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageYourRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageYourRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
