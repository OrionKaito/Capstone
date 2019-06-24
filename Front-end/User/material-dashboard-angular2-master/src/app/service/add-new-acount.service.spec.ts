import { TestBed } from '@angular/core/testing';

import { AddNewAcountService } from './add-new-acount.service';

describe('AddNewAcountService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AddNewAcountService = TestBed.get(AddNewAcountService);
    expect(service).toBeTruthy();
  });
});
