import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatDialog, MatDialogConfig } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { LoginService } from 'app/service/login.service';
import { UserRequestComponent } from 'app/user-request/user-request.component';
import { AddNewRequestComponent } from 'app/add-new-request/add-new-request.component';

@Component({
  selector: 'app-workflow-of-user',
  templateUrl: './workflow-of-user.component.html',
  styleUrls: ['./workflow-of-user.component.scss']
})
export class WorkflowOfUserComponent implements OnInit {
  users: any = [];
  dataGet: any = [];
  dataSave: any = [];
  roleData: string
  groupData: string
  searchKey: string;
  value = '';
  listAccountFull = new Array<any>();
  model: any = {};
  errorMessage: string;
  listData: MatTableDataSource<any>;
  displayedColumns: string[] = ['name', "isDeleted"];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private toastr: ToastrService, private router: Router, private dialog: MatDialog,
    private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService) { }

  ngOnInit() {
    this.callAll();
  }
  callAll() {
    this.loadStaffAcountService.loadWorkflowForUserData().toPromise().then(data => {
      this.users = data;
      this.listData = new MatTableDataSource(this.users);
      this.listData.sort = this.sort;
      this.listData.paginator = this.paginator;

    })
  }

  onSearchClear() {
    this.searchKey = "";
    this.applyFilter();
  }
  applyFilter() {
    this.listData.filter = this.searchKey.trim().toLocaleLowerCase();
  }
 
  CreateRequestOfWF(id: string) {

    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.width = "50%";
    dialogConfig.data = id;
    this.dialog.open(AddNewRequestComponent, dialogConfig).afterClosed().subscribe(res => {
      console.log(res);
      this.callAll();
    });
    this.callAll();
  }


}
