import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatDialog, MatDialogConfig } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { LoginService } from 'app/service/login.service';
import { AddHandleRequestComponent } from 'app/add-handle-request/add-handle-request.component';

@Component({
  selector: 'app-handle-request',
  templateUrl: './handle-request.component.html',
  styleUrls: ['./handle-request.component.scss']
})
export class HandleRequestComponent implements OnInit {
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
  displayedColumns: string[] = ['name',"time", "isDeleted"];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private toastr: ToastrService, private router: Router, private dialog: MatDialog,
    private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService) { }

  ngOnInit() {
    this.callAll();
  }
  callAll() {
    this.loadStaffAcountService.loadHandlingRequest().toPromise().then(data => {
      console.log(data);
      this.users = data;
      console.log(this.users.requests);
      this.listData = new MatTableDataSource(this.users[0].requests);
      this.listData.sort = this.sort;
      this.listData.paginator = this.paginator;

    },err =>{
      console.log(err);
      this.toastr.error(err.error);
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
    this.dialog.open(AddHandleRequestComponent, dialogConfig).afterClosed().subscribe(res => {
      console.log(res);
      this.callAll();
    });
    this.callAll();
  }

}
