import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DxDrawerModule, DxDrawerComponent, DxDrawerTypes } from 'devextreme-angular/ui/drawer';
import { DxToolbarModule } from 'devextreme-angular/ui/toolbar';
import { DxListModule } from 'devextreme-angular/ui/list';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  templateUrl: './sidebar.html',
  styleUrls: ['./sidebar.scss'],
  imports: [
    CommonModule,
    DxDrawerModule,
    DxToolbarModule,
    DxListModule
  ],
})
export class Sidebar {
  @ViewChild(DxDrawerComponent, { static: false }) drawer!: DxDrawerComponent;

  navigation = [
    { id: 1, text: 'Dashboard', icon: 'home' },
    { id: 2, text: 'Orders', icon: 'cart' },
    { id: 3, text: 'Customers', icon: 'user' },
    { id: 4, text: 'Reports', icon: 'chart' },
    { id: 5, text: 'Settings', icon: 'preferences' },
  ];

  selectedMenu: any = null;

  selectedOpenMode: DxDrawerTypes.OpenedStateMode = 'shrink';
  selectedPosition: DxDrawerTypes.PanelLocation = 'left';
  selectedRevealMode: DxDrawerTypes.RevealMode = 'slide';
  isDrawerOpen = true;

  toggleDrawer() {
    this.isDrawerOpen = !this.isDrawerOpen;
  }

  toolbarContent = [
    {
      widget: 'dxButton',
      location: 'before',
      options: {
        icon: 'menu',
        stylingMode: 'text',
        onClick: () => this.toggleDrawer(),
      },
    },
  ];

  onMenuItemClick(e: any) {
    this.selectedMenu = e.itemData;
    this.toggleDrawer();
  }
}
