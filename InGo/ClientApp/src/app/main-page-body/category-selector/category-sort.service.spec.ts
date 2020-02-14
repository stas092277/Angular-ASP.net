import { TestBed } from '@angular/core/testing';

import { CategorySortService } from './category-sort.service';

describe('CategorySortService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CategorySortService = TestBed.get(CategorySortService);
    expect(service).toBeTruthy();
  });
});
