import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MainPageBodyComponent } from './main-page-body.component';

describe('MainPageBodyComponent', () => {
  let component: MainPageBodyComponent;
  let fixture: ComponentFixture<MainPageBodyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MainPageBodyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MainPageBodyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
