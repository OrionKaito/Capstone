import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HandleRequestComponent } from './handle-request.component';

describe('HandleRequestComponent', () => {
  let component: HandleRequestComponent;
  let fixture: ComponentFixture<HandleRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HandleRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HandleRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
