import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAboutPanelComponent } from './user-about-panel.component';

describe('UserAboutPanelComponent', () => {
  let component: UserAboutPanelComponent;
  let fixture: ComponentFixture<UserAboutPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserAboutPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAboutPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
