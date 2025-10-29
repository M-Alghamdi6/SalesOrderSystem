import { Component, OnInit } from '@angular/core';
import { DxDataGridModule, DxPopupModule, DxFormModule } from 'devextreme-angular';
import { CommonModule } from '@angular/common';
import notify from 'devextreme/ui/notify';
import { SalesRequestService, SalesRequesterTableDTO, CreateSalesRequesterDTO } from '../../services/sales-request-service';

@Component({
  selector: 'app-requester-dash-board',
  standalone: true,
  imports: [DxDataGridModule, DxPopupModule, DxFormModule, CommonModule],
  templateUrl: './requester-dash-board.html',
  styleUrls: ['./requester-dash-board.scss']
})
export class RequesterDashBoardComponent implements OnInit {
  salesRequests: SalesRequesterTableDTO[] = [];
  isLoading = false;

  // Popup state
  popupVisible = false;
  newRequest: CreateSalesRequesterDTO = {
    salesNote: '',
    approver: '',
    salesDate: new Date().toISOString().split('T')[0]
  };

  items: any[] = [];

  constructor(private salesRequestService: SalesRequestService) {}

  ngOnInit(): void {
    this.loadSalesRequests();
  }

  loadSalesRequests(): void {
  this.isLoading = true;
  this.salesRequestService.getAllSalesRequests().subscribe({
    next: (res: any) => {
      // If backend wraps the data in res.data, use it
      const data: SalesRequesterTableDTO[] = Array.isArray(res.data)
        ? res.data
        : Array.isArray(res)
        ? res
        : [];

      this.salesRequests = data.map(r => ({
        ...r,
        actions: this.getAvailableActions(r.status)
      }));

      this.isLoading = false;
    },
    error: (err) => {
      console.error('Error fetching requests:', err);
      notify('Failed to load sales requests', 'error', 2000);
      this.isLoading = false;
    }
  });
}


  getAvailableActions(status: string): string {
    const s = status?.toLowerCase() || '';
    return (s === 'approved' || s === 'rejected') ? 'View,Cancel' : 'View,Delete';
  }

  openCreatePopup(): void {
    this.newRequest = {
      salesNote: '',
      approver: '',
      salesDate: new Date().toISOString().split('T')[0]
    };
    this.popupVisible = true;
  }

  saveNewRequest(): void {
    if (!this.newRequest.salesNote) {
      notify('Please enter a sales note', 'warning', 2000);
      return;
    }

    this.salesRequestService.createSalesRequest(this.newRequest).subscribe({
      next: () => {
        notify('Sales Request created successfully', 'success', 2000);
        this.popupVisible = false;
        this.loadSalesRequests();
      },
      error: (err) => {
        console.error('Error creating request:', err);
        notify('Failed to create Sales Request', 'error', 2000);
      }
    });
  }

  onActionClick(action: string, data: any): void {
    const request: SalesRequesterTableDTO = data.data;
    switch (action) {
      case 'View': this.viewRequest(request); break;
      case 'Delete': this.deleteRequest(request.id); break;
      case 'Cancel': this.cancelRequest(request.id); break;
    }
  }

  viewRequest(request: SalesRequesterTableDTO): void {
    this.items = request.salesRequestItems || [];
    notify(`Viewing Sales Request ${request.id}`, 'info', 2000);
  }

  deleteRequest(id: number): void {
    if (!confirm('Are you sure you want to delete this Sales Request?')) return;
    this.salesRequestService.deleteSalesRequest(id).subscribe({
      next: () => { notify('Sales Request deleted successfully', 'success', 2000); this.loadSalesRequests(); },
      error: (err) => { console.error(err); notify('Failed to delete Sales Request', 'error', 2000); }
    });
  }

  cancelRequest(id: number): void {
    if (!confirm('Are you sure you want to cancel this Sales Request?')) return;
    this.salesRequestService.cancelSalesRequest(id).subscribe({
      next: () => { notify('Sales Request cancelled successfully', 'success', 2000); this.loadSalesRequests(); },
      error: (err) => { console.error(err); notify('Failed to cancel Sales Request', 'error', 2000); }
    });
  }
}
