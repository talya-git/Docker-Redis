import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListCustomer } from './list-customer';

describe('ListCustomer', () => {
  let component: ListCustomer;
  let fixture: ComponentFixture<ListCustomer>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListCustomer]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListCustomer);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
