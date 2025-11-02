import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApproverDashboardComponent } from './approver-dashboard-component';

describe('ApproverDashboardComponent', () => {
  let component: ApproverDashboardComponent;
  let fixture: ComponentFixture<ApproverDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApproverDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApproverDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
