import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AsyncPipe, NgClass } from '@angular/common';
import {
  DxDataGridModule,
  DxButtonModule,
  DxTemplateModule,
  DxLoadPanelModule,
  DxTextBoxModule,
  DxDateBoxModule,
  DxTextAreaModule,
  DxSelectBoxModule
} from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { ApiService, SalesRequestDTO } from '../../../services/ApiService';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Component({
  selector: 'app-create-request',
  templateUrl: 'create-order.html',
  styleUrls: ['create-order.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    AsyncPipe,
    NgClass,
    DxDataGridModule,
    DxButtonModule,
    DxTemplateModule,
    DxLoadPanelModule,
    DxTextBoxModule,
    DxDateBoxModule,
    DxTextAreaModule,
    DxSelectBoxModule
  ]
})
export class CreateRequestComponent implements OnInit {
  requestForm: FormGroup;
  loading = false;
  requests$: Observable<SalesRequestDTO[]> = of([]);
  approvers: { id: number, userName: string }[] = [];

  constructor(private fb: FormBuilder, private apiService: ApiService) {
    this.requestForm = this.fb.group({
      sr: [{ value: '', disabled: true }, Validators.required],
      salesDate: ['', Validators.required],
      salesNote: ['', [Validators.required, Validators.maxLength(200)]],
      approver: ['', Validators.required],
      status: [{ value: 'Pending', disabled: true }],
      reason: [{ value: '', disabled: true }]
    });
  }

  ngOnInit(): void {
    this.loadRequests();
    this.generateSR();
    this.loadApprovers();
  }

  // Load existing requests
  loadRequests(): void {
    this.loading = true;
    this.requests$ = this.apiService.getSalesRequests().pipe(
      tap(() => this.loading = false),
      catchError(err => {
        notify(err.message || 'Failed to load requests', 'error', 2000);
        this.loading = false;
        return of([]);
      })
    );
  }

  // Generate SR based on last request ID
  generateSR(): void {
    this.apiService.getSalesRequests().pipe(
      tap((requests: SalesRequestDTO[]) => {
        const lastId = requests.length > 0
          ? Math.max(...requests.map(r => r.id))
          : 0;

        const nextId = lastId + 1;
        const sr = `SR-${nextId.toString().padStart(5, '0')}`;
        this.requestForm.patchValue({ sr, status: 'Pending' });
      }),
      catchError(err => {
        notify('Failed to generate SR', 'error', 2000);
        return of(null);
      })
    ).subscribe();
  }

  // Load approvers from backend
  loadApprovers(): void {
    this.apiService.getApprovers().pipe(
      tap((data: any[]) => {
        this.approvers = data;
        if (data.length > 0) {
          this.requestForm.patchValue({ approver: data[0].userName });
        }
      }),
      catchError(err => {
        notify('Failed to load approvers', 'error', 2000);
        return of([]);
      })
    ).subscribe();
  }

  // Submit request
  submit(): void {
  if (this.requestForm.invalid) {
    notify('Please fill all required fields', 'warning', 2000);
    return;
  }

  const formValue = this.requestForm.getRawValue(); // includes disabled fields

  const request = {
    SalesRequestNo: formValue.salesRequestNo,  // <-- send SR generated above
    SalesDate: formValue.salesDate instanceof Date
                ? formValue.salesDate.toISOString()
                : new Date(formValue.salesDate).toISOString(), // send selected date
    SalesNote: formValue.salesNote,
    Approver: formValue.approver,
    Status: formValue.status,
    Reason: formValue.reason || `Reason provided by ${formValue.approver}`,
    UserId: 1, // replace with actual logged-in user id
    RequesterUsername: 'currentUser',
    SalesRequestItems: []
  };

  this.loading = true;

  this.apiService.createSalesRequest(request).pipe(
    tap(() => {
      notify('Sales request created successfully', 'success', 2000);
      this.requestForm.get('salesDate')?.reset();
      this.requestForm.get('salesNote')?.reset();
      this.generateSR();   // generate next SR
      this.loadApprovers(); // refresh approvers
      this.loadRequests();  // refresh table
    }),
    catchError(err => {
      notify(err.error?.message || 'Failed to create request', 'error', 2000);
      return of(null);
    }),
    tap(() => this.loading = false)
  ).subscribe();
}

  // Status badge CSS
  getStatusBadgeClass(status: string): string {
    switch (status) {
      case 'Approved': return 'badge-approved';
      case 'Rejected': return 'badge-rejected';
      case 'Pending': return 'badge-pending';
      default: return '';
    }
  }
}
