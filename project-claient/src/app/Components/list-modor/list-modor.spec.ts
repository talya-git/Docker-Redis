import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListModor } from './list-modor';

describe('ListModor', () => {
  let component: ListModor;
  let fixture: ComponentFixture<ListModor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListModor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListModor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
