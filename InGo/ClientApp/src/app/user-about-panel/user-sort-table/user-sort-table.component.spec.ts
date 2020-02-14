import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserSortTableComponent } from './user-sort-table.component';

describe('UserSortTableComponent', () => {
  let component: UserSortTableComponent;
  let fixture: ComponentFixture<UserSortTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserSortTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserSortTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
