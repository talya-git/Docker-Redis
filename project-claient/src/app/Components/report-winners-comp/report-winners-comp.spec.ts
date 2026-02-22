import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportWinnersComp } from './report-winners-comp';

describe('ReportWinnersComp', () => {
  let component: ReportWinnersComp;
  let fixture: ComponentFixture<ReportWinnersComp>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReportWinnersComp]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReportWinnersComp);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
