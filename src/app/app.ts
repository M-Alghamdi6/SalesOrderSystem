import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LogInForm } from './Pages/log-in-form/log-in-form';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LogInForm],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('SalesOrderSystem');
}
