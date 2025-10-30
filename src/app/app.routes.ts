import { Routes } from '@angular/router';
import { LogInForm } from '../app/log-in-form/log-in-form';
import { AuthGuard } from './Guard/auth-guard';
import { RequesterDashboardComponent } from './Component/requester-dashboard/requester-dashboard-component/requester-dashboard-component';
import { ApproverDashboardComponent } from './Component/approver-dashboard/approver-dashboard-component/approver-dashboard-component';
export const routes: Routes = [
  { path: 'login', component: LogInForm },

  {
    path: 'requester',
    component: RequesterDashboardComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Requester'] }
  },
  {
    path: 'approver',
    component: ApproverDashboardComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Approver'] }
  },

  { path: '', redirectTo: 'login', pathMatch: 'full' },
 
];
