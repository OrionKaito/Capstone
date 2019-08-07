import { Component, OnInit, ViewChild } from '@angular/core';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { LoginService } from 'app/service/login.service';
import { Router } from '@angular/router';
import { MatTableDataSource, MatSort, MatPaginator, MatDialogConfig, MatDialog, MatDialogActions } from '@angular/material';
import { AddAccountComponent } from 'app/add-account/add-account.component';
import { LoadAccountUser } from 'app/useClass/load-account-user';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-table-list',
  templateUrl: './table-list.component.html',
  styleUrls: ['./table-list.component.css']
})
export class TableListComponent implements OnInit {
  users1: any;
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
  displayedColumns: string[] = ['fullName', "role", "group", "manage", "isDeleted"];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private toastr: ToastrService, private router: Router, private dialog: MatDialog,
    private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService) { }

  ngOnInit() {
    this.callAll();
  }
  callAll() {
    this.loadStaffAcountService.loadStaffData().toPromise().then(data => {

      this.users1 = data;
      this.users = this.users1.accounts;
      this.listData = new MatTableDataSource(this.users);
      console.log(this.listData);
      console.log('sss');
      debugger;
  
      
      const nowData = this.listData.data;
      this.listData.sort = this.sort;
      this.listData.paginator = this.paginator;

      // nowData.forEach(
      //   (obj, index) => {
      //     var a: string;
      //     var b;
      //     this.loadStaffAcountService.getGroupbyID(obj.id).toPromise().then(data => {

      //       this.dataGet = data;
      //       if (this.dataGet != null) {
      //           this.dataGet.forEach(element => {
      //             a += element.name + " & " ;
      //           });
      //           a = a.substring(a.length -3);
      //       }
      //       a = this.dataGet[0];
      //       this.loadStaffAcountService.getRolebyID(obj.id).toPromise().then(data => {

      //         b = data;
      //         this.dataSave.push(b);
      //         this.listAccountFull.push(new LoadAccountUser(obj.id, obj.fullName, obj.email,
      //           obj.dateOfBirth, obj.isDeleted,
      //           b, a));
      //         this.listData = new MatTableDataSource(this.listAccountFull);
      //         this.listData.sort = this.sort;
      //         this.listData.paginator = this.paginator;
      //       })
      //     })
      //   });
    }, err=>{
      console.log(err.message);
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
        this.toastr.success('Success!', 'Toggle role account success!');
        this.callAll();
      },
      err => {
        this.toastr.success('Success!', 'Toggle role account success!');
        this.callAll();
      }
    )
   
  };
  register() {
    this.model.dateOfBirth = this.model.dateOfBirth.toString() + "T06:08:08-05:00";
    console.log(this.model);
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
        this.errorMessage = error.message;
      });
  };
  AddAccountItem(id: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.width = "50%";
    dialogConfig.data = id;
    this.dialog.open(AddAccountComponent, dialogConfig).afterClosed().subscribe(res => {
      this.callAll();
    });
  }
}

