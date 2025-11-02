import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sidebar } from './Layout/SideBar/sidebar/sidebar';
import { CreateRequestComponent } from './Component/create-order/create-order/create-order';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Sidebar, CreateRequestComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('SalesOrderSystem');
}
