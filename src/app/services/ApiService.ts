import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

// ------------------ DTOs ------------------

// Line item DTO
export interface SalesRequestLineDTO {
  lineNumber?: number;
  itemCode: string;
  itemDescription: string;
  itemUnit: string;
  quantity: number;
  price: number;
  amount?: number;
}

// Minimal DTO for creating sales requests
export interface CreateSalesRequestDTO {
  SalesRequestNo: string;
  SalesDate: string;
  SalesNote: string;
  Approver: string;
  Status: string;
  Reason?: string;
  UserId: number;
  RequesterUsername: string;
  SalesRequestItems?: SalesRequestLineDTO[];
}

// Full DTO from backend
export interface SalesRequestDTO {
  id: number;
  salesRequestNo: string;
  salesDate: string;
  salesNote: string;
  approver: string;
  status: string;
  rejectionRemark?: string | null;
  approvalRemark?: string | null;
  requesterUsername: string;
  reason?: string | null;
  userId: number;
  salesRequestItems?: SalesRequestLineDTO[];
}

// ------------------ Service ------------------

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'http://localhost:5035/api/SalesRequester';

  constructor(private http: HttpClient) {}

  // ----------------- GET All -----------------
  getSalesRequests(): Observable<SalesRequestDTO[]> {
    return this.http.get<{ statusCode: number; data: SalesRequestDTO[] }>(this.apiUrl).pipe(
      map(response => response?.data || []),
      catchError(err => {
        console.error('Failed to load sales requests', err);
        return throwError(() => new Error(err?.error?.message || 'Failed to load sales requests'));
      })
    );
  }

  // ----------------- CREATE -----------------
createSalesRequest(request: CreateSalesRequestDTO): Observable<SalesRequestDTO> {
  return this.http.post<{ statusCode: number; data: SalesRequestDTO }>(
    this.apiUrl,
    request,
    { withCredentials: true }
  ).pipe(
    map(res => res.data),
    catchError(err => throwError(() => new Error(err?.error?.message || 'Failed to create request')))
  );
}

getApprovers(): Observable<{ id: number; userName: string }[]> {
  return this.http.get<{ id: number; userName: string }[]>('http://localhost:5035/api/Users/approvers');
}


  // ----------------- DELETE -----------------
  deleteSalesRequest(id: number): Observable<void> {
    return this.http.delete<{ statusCode: number; message: string }>(`${this.apiUrl}/${id}`).pipe(
      map(() => void 0),
      catchError(err => {
        console.error(`Failed to delete sales request ${id}`, err);
        return throwError(() => new Error(err?.error?.message || 'Failed to delete sales request'));
      })
    );
  }

  // ----------------- CANCEL -----------------
  cancelSalesRequest(id: number): Observable<void> {
    return this.http.post<{ statusCode: number; message: string }>(`${this.apiUrl}/${id}/cancel`, {}).pipe(
      map(() => void 0),
      catchError(err => {
        console.error(`Failed to cancel sales request ${id}`, err);
        return throwError(() => new Error(err?.error?.message || 'Failed to cancel sales request'));
      })
    );
  }

  // ----------------- APPROVE -----------------
  approveSalesRequest(id: number, reason?: string): Observable<SalesRequestDTO> {
    return this.http.put<{ statusCode: number; data: SalesRequestDTO }>(`${this.apiUrl}/${id}/approve`, { reason }).pipe(
      map(response => response?.data),
      catchError(err => {
        console.error(`Failed to approve sales request ${id}`, err);
        return throwError(() => new Error(err?.error?.message || 'Failed to approve sales request'));
      })
    );
  }

  // ----------------- REJECT -----------------
  rejectSalesRequest(id: number, reason: string): Observable<SalesRequestDTO> {
    return this.http.put<{ statusCode: number; data: SalesRequestDTO }>(`${this.apiUrl}/${id}/reject`, { reason }).pipe(
      map(response => response?.data),
      catchError(err => {
        console.error(`Failed to reject sales request ${id}`, err);
        return throwError(() => new Error(err?.error?.message || 'Failed to reject sales request'));
      })
    );
  }
  // Add this method
getAutoSalesRequestNo(): Observable<{ sr: string; approver: string }> {
  return this.http.get<{ sr: string; approver: string }>(`${this.apiUrl}/next-sr`);
}

}
