import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { element } from '@angular/core/src/render3';
import { MatTableDataSource, MatSort, MatPaginator, MatDialog, MatDialogConfig } from '@angular/material';
import { AddRoleComponent } from 'app/add-role/add-role.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-manage-role',
  templateUrl: './manage-role.component.html',
  styleUrls: ['./manage-role.component.scss']
})
export class ManageRoleComponent implements OnInit {
  roleGets: any = [];
  value = '';
  searchKey: string;

  model: any = {};
  errorMessage: string;
  listData: MatTableDataSource<any>;
  displayedColumns: string[] = ['roleName', "isDeleted"];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private toastr: ToastrService, private router: Router, private dialog: MatDialog, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() {
    this.startAll()
  }
  startAll() {
    this.loadStaffAcountService.loadRoleData().subscribe(data => {
      this.roleGets = data;
      this.listData = new MatTableDataSource(this.roleGets);
      this.listData.sort = this.sort;
      this.listData.paginator = this.paginator;
    })
  }
  banOrUnbanAcc(id) {
    this.loadStaffAcountService.deleteRole(id).toPromise().then(
      data => {
        this.listData = new MatTableDataSource();
        console.log("ádasdasd");
        this.startAll();
      
      },
      err => {
        this.listData = new MatTableDataSource();
        this.startAll();
        console.log(err);
      }
    )
   
    this.toastr.success("Success!")

    
    
  };
  onSearchClear() {
    this.searchKey = "";
    this.applyFilter();
  }
  applyFilter() {
    this.listData.filter = this.searchKey.trim().toLocaleLowerCase();
  }
  AddOrEditItem(id: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.width = "50%";
    dialogConfig.data = id;
    this.dialog.open(AddRoleComponent, dialogConfig).afterClosed().toPromise().then(res => {
      console.log(res);
      this.startAll();
    });
  }

}
