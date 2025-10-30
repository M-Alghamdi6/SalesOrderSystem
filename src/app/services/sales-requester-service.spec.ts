import { TestBed } from '@angular/core/testing';

import { SalesRequesterService } from './sales-requester-service';

describe('SalesRequesterService', () => {
  let service: SalesRequesterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SalesRequesterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
