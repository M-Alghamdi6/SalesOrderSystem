import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common'; // <-- import this
import { DxFormModule } from 'devextreme-angular/ui/form';
import { DxTextBoxModule } from 'devextreme-angular/ui/text-box';
import { DxButtonModule } from 'devextreme-angular/ui/button';

@Component({
  selector: 'app-log-in-form',
  standalone: true,
  imports: [
    CommonModule,        // <-- add this
    HttpClientModule,    // <-- for HttpClient
    RouterModule,        // <-- for navigation
    DxFormModule,
    DxTextBoxModule,
    DxButtonModule
  ],
  templateUrl: './log-in-form.html',
  styleUrls: ['./log-in-form.scss'],
})
export class LogInForm {
  loginData = { username: '', password: '' };
  errorMessage = '';
    successMessage = '';

  constructor(private http: HttpClient, private router: Router) {}

  login() {
    const payload = {
      UserName: this.loginData.username,
      Password: this.loginData.password
    };

    this.http.post<any>('http://localhost:5035/api/users/login', payload, { withCredentials: true })
      .subscribe({
        next: (res) => {
          if (res.StatusCode === 200) {
            localStorage.setItem('userRole', res.Data.Role);
            this.router.navigate(['/dashboard']);
          } else {
            this.errorMessage = res.Message;
          }
        },
        error: (err) => {
          this.errorMessage = err.error?.Message || 'Login failed';
        }
      });
  }
}
