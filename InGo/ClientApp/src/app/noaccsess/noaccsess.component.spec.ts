import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NoaccsessComponent } from './noaccsess.component';

describe('NoaccsessComponent', () => {
  let component: NoaccsessComponent;
  let fixture: ComponentFixture<NoaccsessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NoaccsessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NoaccsessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
