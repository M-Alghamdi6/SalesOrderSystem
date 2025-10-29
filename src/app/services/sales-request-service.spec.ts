import { TestBed } from '@angular/core/testing';

import { SalesRequestService } from './sales-request-service';

describe('SalesRequestService', () => {
  let service: SalesRequestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SalesRequestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
