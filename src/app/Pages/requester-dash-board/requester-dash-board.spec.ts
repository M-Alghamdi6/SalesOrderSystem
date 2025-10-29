import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequesterDashBoard } from './requester-dash-board';

describe('RequesterDashBoard', () => {
  let component: RequesterDashBoard;
  let fixture: ComponentFixture<RequesterDashBoard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RequesterDashBoard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RequesterDashBoard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
