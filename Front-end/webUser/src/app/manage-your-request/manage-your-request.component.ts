import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatDialog, MatDialogConfig } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { LoginService } from 'app/service/login.service';
import { AddYourRequestComponent } from 'app/add-your-request/add-your-request.component';

@Component({
  selector: 'app-manage-your-request',
  templateUrl: './manage-your-request.component.html',
  styleUrls: ['./manage-your-request.component.scss']
})
export class ManageYourRequestComponent implements OnInit {
  requests: any = [];
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
  displayedColumns: string[] = ['namewf', 'name', 'status', 'createTime', "isDeleted"];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private toastr: ToastrService, private router: Router, private dialog: MatDialog,
    private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService) { }

  ngOnInit() {
    this.callAll();
  }
  callAll() {
    this.loadStaffAcountService.loadYourRequest().toPromise().then(data => {

      this.requests = data;
      let a=this.requests.myRequests;
      this.listData = new MatTableDataSource(a);
      this.listData.sort = this.sort;
      this.listData.paginator = this.paginator;
    },err =>{
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
  banOrUnbanAcc(id) {
    this.LoginService.BanOrUnbanAcc(id).subscribe(
      data => {
        console.log(data);

      },
      err => {
        console.log(err);
      }
    )
    location.reload();
  };
  register() {
    this.model.dateOfBirth = this.model.dateOfBirth.toString() + "T06:08:08-05:00";
    this.LoginService.Register(this.model).subscribe(
      resp => {
        console.log(resp.toString());
        debugger;

        if (resp != "") {
          debugger;
          location.reload();
        }
        else {
          this.errorMessage = resp.toString();
        }
      },
      error => {
        this.errorMessage = error.console.error();
        ;
      });
  };
  SeeFullRequest(id: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.width = "50%";
    dialogConfig.data = id;
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.maxHeight ="80%",
    // dialogConfig.maxHeight = "inherit";
    // dialogConfig.scrollStrategy = true;
    // dialogConfig.autoFocus = false;
    
    this.dialog.open(AddYourRequestComponent, dialogConfig ).afterClosed().subscribe(res => {
      console.log(res);
      this.callAll();
    });
    this.callAll();
  }
}
