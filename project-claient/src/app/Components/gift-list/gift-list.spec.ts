import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftList } from './gift-list';

describe('GiftList', () => {
  let component: GiftList;
  let fixture: ComponentFixture<GiftList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiftList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GiftList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
