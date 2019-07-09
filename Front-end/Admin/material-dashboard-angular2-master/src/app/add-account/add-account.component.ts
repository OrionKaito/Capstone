import { Component, OnInit, Inject } from '@angular/core';
import { AccountItem } from 'app/useClass/account-item';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { Router } from '@angular/router';
import { inject } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { LoginService } from 'app/service/login.service';
import { identifierModuleUrl } from '@angular/compiler';

@Component({
  selector: 'app-add-account',
  templateUrl: './add-account.component.html',
  styleUrls: ['./add-account.component.scss']
})
export class AddAccountComponent implements OnInit {
  formData = new AccountItem;
  dataGet: any = [];
  recevieData1: any
  recevieData: any
  inputRole
  inputGroup
  createAcc = true;

  disable = false;
  showFilter = true;
  limitSelection = false;
  selectItem: any = [];
  dropDownSetting: any = {};
  constructor(
    @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddAccountComponent>,
    private router: Router, private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService) { }
  roleList: any = [];
  groupList: any = [];
  manageList: any = [];
  manageList1: any = [];

  ngOnInit() {

    this.formData.groupIDs[0] = "0";
    this.formData.roleIDs[0] = "0";
    this.formData.manageID = "0";
    // this.inputGroup=[{
    //   "id": "e6f8b263-9806-4198-1d2d-08d6f6159d78",
    //   "name": "Phòng quản lý"
    // }]
    this.loadStaffAcountService.loadGroupData().toPromise().then(data => {
      this.groupList = data;
      console.log(this.groupList);
      this.dropDownSetting = {
        singleSelection: false, primaryKey: 'id', labelKey: 'name', selectAlltext: 'Select All',
        unSelectAlltext: 'UnSelect All', itemShowLimit: 3, enableSearchFilter: this.showFilter
      };
      this.loadStaffAcountService.loadRoleData().toPromise().then(data => {
        this.roleList = data;
        this.loadStaffAcountService.loadStaffData().toPromise().then(data => {
          this.manageList1 = data;
          this.manageList = this.manageList1.accounts;
          if (this.data != null && this.data != "null") this.createAcc = false;
          if (!this.createAcc) {

            this.loadStaffAcountService.loadUserByID(this.data).toPromise().then(data => {
              this.recevieData1 = data;
              this.recevieData = this.recevieData1[0];
              this.formData.email = this.recevieData.email;
              this.formData.dateOfBirth = this.recevieData.dateOfBirth;
              this.formData.fullName = this.recevieData.fullName;
              this.formData.groupIDs = this.recevieData.groups;
              this.formData.roleIDs = this.recevieData.roles;
              this.formData.manageID = this.recevieData.managerID;
              console.log(this.formData);
            })
          }
        })
      })

    })
  }
  onSubmit() {
    if (this.createAcc) {
      var a = this.formData.dateOfBirth.toString() + "T06:08:08-05:00";
      this.formData.dateOfBirth = new Date(a);
      this.LoginService.Register(this.formData).subscribe(
        resp => {
          console.log(resp.toString());
          debugger;
          if (resp != "") {
            debugger;
          }
          else {
            //this.errorMessage = resp.toString();    
          }
        });

    } else {
      console.log(this.formData);
      this.LoginService.Register(this.formData).subscribe(
        resp => {
          console.log(resp.toString());
          debugger;
          if (resp != "") {
            debugger;
          }
          else {
            //this.errorMessage = resp.toString();    
          }
        });
    }
  }

}
