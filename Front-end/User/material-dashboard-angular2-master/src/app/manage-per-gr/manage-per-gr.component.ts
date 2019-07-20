import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatDialog, MatDialogConfig } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { LoginService } from 'app/service/login.service';
import { AddManagePerGrComponent } from 'app/add-manage-per-gr/add-manage-per-gr.component';

@Component({
  selector: 'app-manage-per-gr',
  templateUrl: './manage-per-gr.component.html',
  styleUrls: ['./manage-per-gr.component.scss']
})
export class ManagePerGrComponent implements OnInit {

  users: any = [];
  dataGet: any = [];
  dataSave: any = [];
  listGrPer: any=[];
  roleData: string
  groupData: string
  searchKey: string;
  value = '';
  listAccountFull = new Array<any>();
  model: any = {};
  errorMessage: string;
  listData: MatTableDataSource<any>;
  displayedColumns: string[] = ['perName', 'grName', "isDeleted"];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private toastr: ToastrService, private router: Router, private dialog: MatDialog,
    private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService) { }

  ngOnInit() {
    this.callAll();
  }
  callAll() {
    this.loadStaffAcountService.loadPermissionGroupData().toPromise().then(data => {
      this.listGrPer=[];
      this.users = data;
      console.log(this.users);
     
      this.users.forEach(element => {
        let saveListPerToString="";
        element.permissions.forEach(element2 => {
          saveListPerToString =  saveListPerToString + element2.permissionName+", ";
        });
        saveListPerToString= saveListPerToString.substring(0, saveListPerToString.length-2)
        this.listGrPer.push({nameGr: element.groupName, namePer: saveListPerToString});
      });

      this.listData = new MatTableDataSource(this.listGrPer);

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
  // register() {
  //   this.model.dateOfBirth = this.model.dateOfBirth.toString() + "T06:08:08-05:00";
  //   this.LoginService.Register(this.model).subscribe(
  //     resp => {
  //       console.log(resp.toString());
  //       debugger;

  //       if (resp != "") {
  //         debugger;
  //         location.reload();
  //       }
  //       else {
  //         this.errorMessage = resp.toString();
  //       }
  //     },
  //     error => {
  //       this.errorMessage = error.message;
  //     });
  // };
  AddOrEditWF(id: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.width = "50%";
    dialogConfig.data = id;
    this.dialog.open(AddManagePerGrComponent, dialogConfig).afterClosed().subscribe(res => {
      console.log(res);
      this.callAll();
    });
    this.callAll();
  }

}
