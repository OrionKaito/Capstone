import { Component, OnInit, ViewChild } from '@angular/core';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { Router } from '@angular/router';
import { MatTableDataSource, MatSort, MatPaginator, MatDialog, MatDialogConfig, MatDialogActions } from '@angular/material';
import { AddGroupComponent } from 'app/add-group/add-group.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-manage-group',
  templateUrl: './manage-group.component.html',
  styleUrls: ['./manage-group.component.scss']
}) 
export class ManageGroupComponent implements OnInit {
  groupGets: any= [];
  value = '';
  searchKey: string;
  model : any={}; 
  errorMessage:string; 
  listData: MatTableDataSource<any>;
  displayedColumns: string[] =['groupName', "isDeleted" ];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private router:Router, private toastr: ToastrService, private dialog:MatDialog, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() { 
    this.callAll();
  }
  callAll(){
    this.loadStaffAcountService.loadGroupData().toPromise().then(data => {
      this.groupGets = data;
      this.listData = new MatTableDataSource(this.groupGets); 
      this.listData.sort = this.sort;
      this.listData.paginator = this.paginator;    
    })
  }
  banOrUnbanAcc(id){
    debugger;
     this.loadStaffAcountService.deleteGroup(id).subscribe(
       data=>{
        this.listData = new MatTableDataSource();
         console.log(data);   
         this.callAll()             
       },
       err=>{
        this.listData = new MatTableDataSource();
        this.callAll()
       console.log(err);
     }
       )
       this.toastr.success("Success!")
     
   
 };
 onSearchClear(){
  this.searchKey="";
  this.applyFilter();
}
applyFilter(){
  this.listData.filter = this.searchKey.trim().toLocaleLowerCase();
}
AddOrEditItem(id:string){
  const dialogConfig = new MatDialogConfig();
  dialogConfig.autoFocus = true;
  dialogConfig.disableClose = true;
  dialogConfig.width = "50%";
  dialogConfig.data = id;
  this.dialog.open(AddGroupComponent, dialogConfig).afterClosed().subscribe(res => {
    this.callAll();
    console.log(res);
  });
}
 
}
