import { Component, OnInit } from '@angular/core';
import {
  DxDataGridModule,
  DxButtonModule,
  DxTemplateModule,
  DxLoadPanelModule
} from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { ApiService, SalesRequestDTO } from '../../../services/ApiService';
import { AsyncPipe, NgClass } from '@angular/common';
import { Observable, of } from 'rxjs';
import { catchError, finalize, tap, map } from 'rxjs/operators';

@Component({
  selector: 'app-requester-dashboard',
  templateUrl: 'requester-dashboard-component.html',
  styleUrls: ['requester-dashboard-component.scss'],
  standalone: true,
  imports: [AsyncPipe, NgClass, DxDataGridModule, DxButtonModule, DxTemplateModule, DxLoadPanelModule]
})
export class RequesterDashboardComponent implements OnInit {
  dataSource$: Observable<SalesRequestDTO[]> = of([]);
  loading = false;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
  this.loading = true;
  this.dataSource$ = this.apiService.getSalesRequests().pipe(
    map(requests => requests.map(r => ({
      id: r.id,
      sr: r.salesRequestNo,
      salesDate: r.salesDate,
      salesNote: r.salesNote,
      approver: r.approver,
      status: r.status,
      reason: r.reason,
      actions: '',
      salesRequestNo: r.salesRequestNo,        // add missing fields from DTO
      requesterUsername: r.requesterUsername,
      userId: r.userId,
      rejectionRemark: r.rejectionRemark,
      approvalRemark: r.approvalRemark,
      salesRequestItems: r.salesRequestItems
    }))),
    catchError(err => {
      notify(err.error?.message || err.message || 'Failed to load requests', 'error', 2000);
      this.loading = false;
      return of([]); // fallback to empty array
    }),
    finalize(() => this.loading = false)
  );
}


  // --- Row Button Handlers ---
  onView(e: any): void {
    console.log('View button clicked. Event:', e);
    console.log('View request data:', e.row.data);
  }

  onDelete = (e: any): void => {
    const request = e.row.data;
    console.log('Delete clicked for request:', request);
    if (!request?.id) {
      notify('Request ID invalid or missing', 'error', 2000);
      return;
    }
    this.apiService.deleteSalesRequest(request.id).subscribe({
      next: () => {
        notify('Deleted successfully', 'success', 2000);
        this.loadData();
      },
      error: err => {
        notify(err.message || 'Delete failed', 'error', 2000);
      }
    });
  };

  onCancel = (e: any): void => {
    const request = e.row.data;
    if (request.status !== 'Approved' && request.status !== 'Rejected') {
      notify('Only approved or rejected requests can be cancelled', 'error', 2000);
      return;
    }
    this.apiService.cancelSalesRequest(request.id).subscribe({
      next: () => {
        notify('Cancelled successfully', 'success', 2000);
        this.loadData();
      },
      error: err => {
        notify(err.message || 'Cancel failed', 'error', 2000);
      }
    });
  };

  // --- Helpers ---
  getStatusBadgeClass(status: string): string {
    switch (status) {
      case 'Approved': return 'badge-approved';
      case 'Rejected': return 'badge-rejected';
      case 'Pending': return 'badge-pending';
      default: return '';
    }
  }
}
