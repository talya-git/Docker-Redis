import { TestBed } from '@angular/core/testing';

import { ModorService } from './modor-service';

describe('ModorService', () => {
  let service: ModorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ModorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
