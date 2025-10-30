// sales-requester.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface SalesRequest {
  SR: string;
  SalesDate: Date | null;
  SalesNote: string;
  Approver: string;
  Status: string;
  Reason: string;
  Actions?: string;
}
@Injectable({
  providedIn: 'root'
})
export class SalesRequesterService {
  
  private baseUrl = 'http://localhost:5035/api/SalesRequester';

  constructor(private http: HttpClient) {}

  // إنشاء طلب جديد
  createRequest(requestData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}`, requestData, { withCredentials: true });
  }

  // جلب كل الطلبات الخاصة بالمستخدم
 getUserRequests(): Observable<any> {
  return this.http.get(`${this.baseUrl}`, { withCredentials: true });
}
  // حذف طلب
  deleteRequest(sr: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${sr}`, { withCredentials: true });
  }

  // إلغاء طلب
  cancelRequest(sr: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/${sr}/cancel`, {}, { withCredentials: true });
  }
}