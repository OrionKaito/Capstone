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
    private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService,) { }

  ngOnInit() {
    this.callAll();
  }
  callAll() {
     this.loadStaffAcountService.loadWorkFlow().toPromise().then(data => {
       this.users= data;

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
        this.errorMessage = error.message;
      });
  };
  AddOrEditWF(id: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.width = "50%";
    dialogConfig.data = id;
    this.dialog.open(AddAccountComponent, dialogConfig).afterClosed().subscribe(res => {
      console.log(res);
      this.callAll();
    });
  }
  EditWFAtTool() {

  }

  public navigateEdit(id) {
    // navigate qua trang edit với id của User
    // Mock IdUser

    this.router.navigate(['edit-account/', id]);
  }

}

