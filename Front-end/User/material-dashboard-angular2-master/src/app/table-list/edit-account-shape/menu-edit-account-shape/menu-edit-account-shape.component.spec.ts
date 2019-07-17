import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuEditAccountShapeComponent } from './menu-edit-account-shape.component';

describe('MenuEditAccountShapeComponent', () => {
  let component: MenuEditAccountShapeComponent;
  let fixture: ComponentFixture<MenuEditAccountShapeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MenuEditAccountShapeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuEditAccountShapeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
