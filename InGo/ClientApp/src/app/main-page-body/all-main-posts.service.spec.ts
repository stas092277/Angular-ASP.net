import { TestBed } from '@angular/core/testing';

import { AllMainPostsService } from './all-main-posts.service';

describe('AllMainPostsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AllMainPostsService = TestBed.get(AllMainPostsService);
    expect(service).toBeTruthy();
  });
});
