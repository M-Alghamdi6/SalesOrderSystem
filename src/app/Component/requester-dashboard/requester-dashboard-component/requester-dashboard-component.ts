// requester-dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { SalesRequesterService } from '../../../services/sales-requester-service';
import notify from 'devextreme/ui/notify';
import { DxDataGridModule, DxButtonModule, DxTemplateModule, DxLoadPanelModule } from 'devextreme-angular';

export interface SalesRequest {
  SR: string;
  SalesDate: Date | null;
  SalesNote: string;
  Approver: string;
  Status: string;
  Reason: string;
  Actions?: string;
}
@Component({
  selector: 'app-requester-dashboard',
  templateUrl: './requester-dashboard-component.html',
  styleUrls: ['./requester-dashboard-component.scss'],
  standalone: true,
imports: [DxDataGridModule, DxButtonModule, DxTemplateModule, DxLoadPanelModule]


})
export class RequesterDashboardComponent implements OnInit {
  requests: SalesRequest[] = [];
  loading = false;

  constructor(private service: SalesRequesterService) {}

  ngOnInit() {
    this.loadRequests();
  }

  loadRequests() {
    this.loading = true;
    this.service.getUserRequests().subscribe({
      next: (res: any) => {
        this.requests = (res.data ?? []).map((r: any) => ({
          SR: r.sr || r.SalesRequestNo || '',
          SalesDate: r.salesDate ? new Date(r.salesDate) : null,
          SalesNote: r.salesNote || '',
          Approver: r.approver || '',
          Status: r.status || '',
          Reason: r.reason || '',
          Actions: ''
        }));
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        notify('Failed to load requests', 'error', 2000);
      }
    });
  }

  onDelete(request: SalesRequest) {
  if (request.Status === 'Approved' || request.Status === 'Rejected') {
    notify('Cannot delete approved or rejected requests', 'error', 2000);
    return;
  }

  this.service.deleteRequest(request.SR).subscribe({
    next: () => {
      notify('Request deleted successfully', 'success', 2000);
      this.loadRequests();
    },
    error: (err) => notify(err.error?.Message || 'Delete failed', 'error', 2000)
  });
}

onCancel(request: SalesRequest) {
  if (request.Status !== 'Approved' && request.Status !== 'Rejected') {
    notify('Only approved or rejected requests can be cancelled', 'error', 2000);
    return;
  }

  this.service.cancelRequest(request.SR).subscribe({
    next: () => {
      notify('Request cancelled successfully', 'success', 2000);
      this.loadRequests();
    },
    error: (err) => notify(err.error?.Message || 'Cancel failed', 'error', 2000)
  });
} onView(request: SalesRequest) {
  console.log('View request:', request);
  // Optionally open a popup or route to a detail page
}

onEdit(request: SalesRequest) {
  console.log('Edit request:', request);
  // Optionally open an edit form
} }