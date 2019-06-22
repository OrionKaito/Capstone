import { TestBed } from '@angular/core/testing';

import { LoadStaffAcountService } from './load-staff-acount.service';

describe('LoadStaffAcountService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LoadStaffAcountService = TestBed.get(LoadStaffAcountService);
    expect(service).toBeTruthy();
  });
});

