import { TestBed } from '@angular/core/testing';

import { ChaincheckService } from './chaincheck.service';

describe('ChaincheckService', () => {
  let service: ChaincheckService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChaincheckService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
