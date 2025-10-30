import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
    loading = false;

  constructor(private http: HttpClient, private router: Router) {}

  login(loginData: { username: string; password: string }): Observable<any> {
    const payload = {
      UserName: loginData.username,
      Password: loginData.password
    };
    return this.http.post<any>('http://localhost:5035/api/Users/login', payload, { withCredentials: true });
  }

 handleLoginResponse(res: any) {
  console.log('Login API response:', res);

  // تحقق من وجود role في الرد
  const role = res?.role;
  
  if (role) {
    // حفظ الدور في التخزين المحلي
    localStorage.setItem('userRole', role);

    // التوجيه حسب الدور
    if (role === 'Requester') {
      this.router.navigate(['/requester']);
    } else if (role === 'Approver') {
      this.router.navigate(['/approver']);
    } else {
      this.router.navigate(['/login']); // في حال الدور غير معروف
    }
  } else {
    console.error('Login failed or unexpected response:', res.message);
  }
}



  getUserRole(): string | null {
    return localStorage.getItem('userRole');
  }

  logout() {
    localStorage.removeItem('userRole');
    this.router.navigate(['/login']);
  }
}
