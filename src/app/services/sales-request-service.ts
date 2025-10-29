import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// Full DTO returned from backend
export interface SalesRequesterTableDTO {
  id: number;
  sr: string;
  salesDate: string;
  salesNote: string;
  approver: string;
  status: string;
  reason: string;
  actions: string;
  salesRequestItems?: any[];
}

// DTO used only for creating a new request
export interface CreateSalesRequesterDTO {
  salesNote: string;
  approver?: string;
  salesDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class SalesRequestService {
  private apiUrl = 'http://localhost:5035/api/SalesRequester';

  constructor(private http: HttpClient) {}

 getAllSalesRequests(): Observable<SalesRequesterTableDTO[]> {
  return this.http.get<SalesRequesterTableDTO[]>(this.apiUrl, { withCredentials: true });
}

createSalesRequest(request: CreateSalesRequesterDTO): Observable<SalesRequesterTableDTO> {
  return this.http.post<SalesRequesterTableDTO>(this.apiUrl, request, { withCredentials: true });
}


  deleteSalesRequest(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  cancelSalesRequest(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/cancel`, {});
  }
}
