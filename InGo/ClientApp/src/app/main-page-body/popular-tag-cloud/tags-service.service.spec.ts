import { TestBed } from '@angular/core/testing';

import { TagsServiceService } from './tags-service.service';

describe('TagsServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TagsServiceService = TestBed.get(TagsServiceService);
    expect(service).toBeTruthy();
  });
});
