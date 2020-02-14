import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PopularTagCloudComponent } from './popular-tag-cloud.component';

describe('PopularTagCloudComponent', () => {
  let component: PopularTagCloudComponent;
  let fixture: ComponentFixture<PopularTagCloudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PopularTagCloudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PopularTagCloudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
