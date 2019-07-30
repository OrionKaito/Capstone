import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddYourRequestComponent } from './add-your-request.component';

describe('AddYourRequestComponent', () => {
  let component: AddYourRequestComponent;
  let fixture: ComponentFixture<AddYourRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddYourRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddYourRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
