import { TestBed } from '@angular/core/testing';

import { SlotstoreService } from './slotstore.service';

describe('SlotstoreService', () => {
  let service: SlotstoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SlotstoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
