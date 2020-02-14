import { TestBed } from '@angular/core/testing';

import { EditCommentService } from './edit-comment.service';

describe('EditCommentService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EditCommentService = TestBed.get(EditCommentService);
    expect(service).toBeTruthy();
  });
});
