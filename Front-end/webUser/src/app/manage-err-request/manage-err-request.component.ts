import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatDialog, MatDialogConfig } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { LoginService } from 'app/service/login.service';

@Component({
  selector: 'app-manage-err-request',
  templateUrl: './manage-err-request.component.html',
  styleUrls: ['./manage-err-request.component.scss']
})
export class ManageErrRequestComponent implements OnInit {
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
  displayedColumns: string[] = ['name', 'action', 'initiator', 'reason', 'date'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private toastr: ToastrService, private router: Router, private dialog: MatDialog,
    private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService) { }

  ngOnInit() {
    this.callAll();
  }
  callAll() {
    this.loadStaffAcountService.loadErrRequestData().toPromise().then(data => {
      this.users = data;
      this.listData = new MatTableDataSource(this.users.myRequests);
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

    this.LoginService.BanOrUnbanPer(id).toPromise().then(
      data => {
        this.toastr.success(data);
        this.callAll();
      },
      err => {
        console.log(err);
      }
    )

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
        this.errorMessage = error.error;
      });
  };
  AddOrEditWF(id: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.width = "50%";
    dialogConfig.data = id;
    // this.dialog.open(AddPermissionComponent, dialogConfig).afterClosed().subscribe(res => {
    //   console.log(res);
    //   this.callAll();
    // });
  //   this.callAll();
  // }
}

}
