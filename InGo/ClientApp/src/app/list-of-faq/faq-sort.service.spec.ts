import { TestBed } from '@angular/core/testing';

import { FaqSortService } from './faq-sort.service';

describe('FaqSortService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FaqSortService = TestBed.get(FaqSortService);
    expect(service).toBeTruthy();
  });
});
