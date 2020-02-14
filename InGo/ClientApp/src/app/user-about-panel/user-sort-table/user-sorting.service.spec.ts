import { TestBed } from '@angular/core/testing';

import { UserSortingService } from './user-sorting.service';

describe('UserSortingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserSortingService = TestBed.get(UserSortingService);
    expect(service).toBeTruthy();
  });
});
