import { ComponentFixture, TestBed } from '@angular/core/testing';

import { modor } from './modor';

describe('modor', () => {
  let component: modor;
  let fixture: ComponentFixture<modor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [modor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(modor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
