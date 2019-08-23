import { Component, OnInit } from '@angular/core';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

declare const $: any;
declare interface RouteInfo {
  path: string;
  title: string;
  icon: string;
  class: string;
}
export const ROUTES: RouteInfo[] = [
  // { path: '/dashboard', title: 'Dashboard',  icon: 'dashboard', class: '' },
  // { path: '/manage-workflow', title: 'Manage WorkFlow',  icon:'content_paste', class: '' },
  // { path: '/manage-permission', title: 'Manage Permission',  icon:'content_paste', class: '' },
  // { path: '/your-request', title: 'My request',  icon:'content_paste', class: '' },
  // { path: '/manage-per-gr', title: 'Manage Permission Group',  icon:'content_paste', class: '' },
  // { path: '/create-request', title: 'Create Request',  icon:'content_paste', class: '' },
  // { path: '/handle-request', title: 'Handle Request',  icon:'content_paste', class: '' }
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];
  dataNow: any

  constructor(private toastr: ToastrService, private router: Router,private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() {
    this.loadStaffAcountService.checkRole().toPromise().then(rep => {
      this.dataNow = rep;
      if (this.dataNow.name == "staff") {
        for (let i = 0; i < 10; i++) {
          ROUTES.pop();
        }
        ROUTES.push(
          { path: '/create-request', title: 'Create Request', icon: 'content_paste', class: 'addLine' },
          { path: '/handle-request', title: 'Handle Request', icon: 'content_paste', class: 'addLine' },
          { path: '/your-request', title: 'My request', icon: 'content_paste', class: 'addLine' },
          { path: '/manage-workflow', title: 'Manage WorkFlow', icon: 'content_paste', class: 'addLine' },
          { path: '/manage-permission', title: 'Manage Permission', icon: 'content_paste', class: 'addLine' },
          { path: '/manage-per-gr', title: 'Manage Permission Group', icon: 'content_paste', class: 'addLine' },
          { path: '/err-request', title: 'Request Can Not Handle', icon: 'content_paste', class: 'addLine' }
        );

      } else {
        for (let i = 0; i < ROUTES.length + 5; i++) {
          ROUTES.pop();
        }
        ROUTES.push(

          { path: '/create-request', title: 'Create Request', icon: 'content_paste', class: '' },
          { path: '/handle-request', title: 'Handle Request', icon: 'content_paste', class: '' },
          { path: '/your-request', title: 'My request', icon: 'content_paste', class: '' }
        );

      }
      this.menuItems = ROUTES.filter(menuItem => menuItem);
    }, err => {
      this.router.navigate(['/login']);
    });

   
  }
  isMobileMenu() {
    if ($(window).width() > 991) {
      return false;
    }
    return true;
  };

  moveToHome(){
    this.router.navigate(['/create-request']); 
  }

}
