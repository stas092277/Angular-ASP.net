import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListOfFaqComponent } from './list-of-faq.component';

describe('ListOfFaqComponent', () => {
  let component: ListOfFaqComponent;
  let fixture: ComponentFixture<ListOfFaqComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListOfFaqComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListOfFaqComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
