import { Component, OnInit } from '@angular/core';

declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
}
export const ROUTES: RouteInfo[] = [
    { path: '/dashboard', title: 'Dashboard',  icon: 'dashboard', class: '' },
    { path: '/user-profile', title: 'User Profile',  icon:'person', class: '' },
    { path: '/manage-workflow', title: 'Manage WorkFlow',  icon:'content_paste', class: '' },
    { path: '/manage-permission', title: 'Manage Permission',  icon:'content_paste', class: '' },
    { path: '/manage-per-gr', title: 'Manage Permission Group',  icon:'content_paste', class: '' },
    { path: '/create-request', title: 'Create Request',  icon:'content_paste', class: '' },
    { path: '/handle-request', title: 'Handle Request',  icon:'content_paste', class: '' }
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];

  constructor() { }

  ngOnInit() {
    this.menuItems = ROUTES.filter(menuItem => menuItem);
  }
  isMobileMenu() {
      if ($(window).width() > 991) {
          return false;
      }
      return true;
  };
}
