import { Component, OnInit, Inject } from '@angular/core';
import { AddGroup } from 'app/useClass/add-group';
import { AddGroupIdName } from 'app/useClass/add-group-id-name';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-add-permission',
  templateUrl: './add-permission.component.html',
  styleUrls: ['./add-permission.component.scss']
})
export class AddPermissionComponent implements OnInit {


  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};


  createGroup = true;
  formData = new AddGroup();
  saveData: any;
  saveData1: any;
  formDataEdit = new AddGroupIdName();
  constructor(@Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddPermissionComponent>, private toastr: ToastrService,
    private router: Router, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() {
    this.formData.name = "";
    if (this.data != null && this.data != "null") this.createGroup = false;
    if (!this.createGroup) {
      this.loadStaffAcountService.loadPermissionByID(this.data).toPromise().then(res => {
        this.saveData = res;
        this.formData.name = this.saveData.name;
      },err =>{
        this.toastr.error(err.error);
      })
    }
    // this.loadStaffAcountService.loadGroupData().toPromise().then(res => {
    //   this.saveData1 = res;
    //   this.dropdownList = [
    //   ];
    //   this.saveData1.forEach(element => {
    //     this.dropdownList.push({ item_id: element.id, item_text: element.name });
    //   });

    //   console.log(this.dropdownList);
    // })
    // this.selectedItems = [
    //   // { item_id: 3, item_text: 'Pune' },
    //   // { item_id: 4, item_text: 'Navsari' }
    // ];

    // this.dropdownSettings = {
    //   singleSelection: false,
    //   idField: 'item_id',
    //   textField: 'item_text',
    //   selectAllText: 'Select All',
    //   unSelectAllText: 'UnSelect All',
    //   itemsShowLimit: 3,
    //   allowSearchFilter: true
    // };

  }
  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }
  onSubmit() {
    debugger;
    if (this.formData.name == "") {
      this.toastr.error("Please fill all fields!");
    } else {
      if (this.createGroup) {
        this.loadStaffAcountService.addPermission(this.formData).toPromise().then(
          resp => {
            console.log(resp.toString());

          },err =>{
            this.toastr.error(err.error);
          });

      } else {
        this.formDataEdit.name = this.formData.name;
        this.formDataEdit.id = this.data;
        this.loadStaffAcountService.editPermission(this.formDataEdit).toPromise().then(
          resp => {
            console.log(resp.toString());
            debugger;

            if (resp != "") {
              debugger;
            }
            else {
              //this.errorMessage = resp.toString();    
            }
          },err =>{
            this.toastr.error(err.error);
          });
      }
      this.toastr.success('Success! ', '');
      this.dialogRef.close();
    }
  }



}
