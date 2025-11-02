import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { DxTextBoxModule, DxButtonModule, DxValidatorModule, DxValidationSummaryModule } from 'devextreme-angular';
import { LoginService } from '../services/login-service';
import notify from 'devextreme/ui/notify';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-log-in-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DxTextBoxModule,
    DxButtonModule,
    DxValidatorModule,
    DxValidationSummaryModule,
  ],
  templateUrl: './log-in-form.html',
  styleUrls: ['./log-in-form.scss'],
})
export class LogInForm implements OnInit, OnDestroy {
  loginData = { username: '', password: '' };
  loading = false; 
  service = inject(LoginService);
  private subscription: Subscription | null = null;

  // ----------------- Login Handler -----------------
  onLogin(): void {
    this.loading = true;

    this.subscription = this.service.login(this.loginData).subscribe({
      next: (res: any) => { 
        this.loading = false;
        this.service.handleLoginResponse(res);
      },
      error: (err: HttpErrorResponse) => {
        this.loading = false;
        const errorMsg = err.error?.Message || err.message || 'Unknown error';
        notify('Login failed: ' + errorMsg, 'error', 2000);
        console.error('Login error:', err);
      }
    });
  }

  ngOnInit(): void {}

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
