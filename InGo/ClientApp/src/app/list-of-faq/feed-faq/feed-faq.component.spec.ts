import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedFaqComponent } from './feed-faq.component';

describe('FeedFaqComponent', () => {
  let component: FeedFaqComponent;
  let fixture: ComponentFixture<FeedFaqComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FeedFaqComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeedFaqComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
