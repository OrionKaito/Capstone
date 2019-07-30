import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';

@Component({
  selector: 'app-add-manage-per-gr',
  templateUrl: './add-manage-per-gr.component.html',
  styleUrls: ['./add-manage-per-gr.component.scss']
})
export class AddManagePerGrComponent implements OnInit {

  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};
 
  groupList: any =[];
  permissionList: any =[];

  createGroup = true;
  formData = { groupID: "0",
    permissionID: "0"}
  saveData: any;
  saveData1: any;
 // formDataEdit = new AddGroupIdName();
  constructor(@Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddManagePerGrComponent>, private toastr: ToastrService,
    private router: Router, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() {
    debugger;
    if (this.data != null && this.data != "null") this.createGroup = false;

    this.loadStaffAcountService.loadGroupData().toPromise().then(data=>{
      this.groupList = data;
      this.loadStaffAcountService.loadPermissionData().toPromise().then(res=>{
        this.permissionList = res;
      })
    })
    if (!this.createGroup) {
      this.loadStaffAcountService.loadPermissionByID(this.data).toPromise().then(res => {
        this.saveData = res;
      //  this.formData.name = this.saveData.name;
      })
    }


  }
  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }
  onSubmit() {
    if (this.createGroup) {
      this.loadStaffAcountService.addPermissionGr(this.formData).toPromise().then(
        resp => {
          console.log(resp.toString());
          this.toastr.success("Success");
          this.dialogRef.close()
        });

    } else {
   //   this.formDataEdit.name = this.formData.name;
  //    this.formDataEdit.id = this.data;
      // this.loadStaffAcountService.editPermission(this.formDataEdit).toPromise().then(
      //   resp => {
      //     console.log(resp.toString());
      //     debugger;

      //     if (resp != "") {
      //       debugger;
      //     }
      //     else {
      //       //this.errorMessage = resp.toString();    
      //     }
      //   });
    }

  }

}
