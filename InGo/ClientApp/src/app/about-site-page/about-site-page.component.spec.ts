import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutSitePageComponent } from './about-site-page.component';

describe('AboutSitePageComponent', () => {
  let component: AboutSitePageComponent;
  let fixture: ComponentFixture<AboutSitePageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutSitePageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutSitePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
