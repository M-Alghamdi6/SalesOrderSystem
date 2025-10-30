import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { DxTextBoxModule, DxButtonModule, DxValidatorModule, DxValidationSummaryModule } from 'devextreme-angular';
import { LoginService } from '../services/login-service';
import notify from 'devextreme/ui/notify';

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

  onLogin() {
  this.loading = true;
  this.service.login(this.loginData).subscribe({
    next: (res) => {
      this.loading = false;
      this.service.handleLoginResponse(res);
    },
    error: (err) => {
      this.loading = false;
      notify('Login failed: ' + (err.error?.Message || 'Unknown error'), 'error', 2000);
    }
  });
}


  ngOnInit() {}
  ngOnDestroy() {
    this.subscription?.unsubscribe();
  }
}

