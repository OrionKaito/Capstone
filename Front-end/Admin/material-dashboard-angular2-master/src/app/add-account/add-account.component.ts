import { Component, OnInit, Inject } from '@angular/core';
import { AccountItem } from 'app/useClass/account-item';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { Router } from '@angular/router';
import { inject } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { LoginService } from 'app/service/login.service';
import { identifierModuleUrl } from '@angular/compiler';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-account',
  templateUrl: './add-account.component.html',
  styleUrls: ['./add-account.component.scss']
})
export class AddAccountComponent implements OnInit {
  formData = new AccountItem([],[],"","","","","");
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
    @Inject(MAT_DIALOG_DATA) public data, private toastr: ToastrService,
    public dialogRef: MatDialogRef<AddAccountComponent>,
    private router: Router, private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService) { }
  roleList: any = [];
  groupList: any = [];
  dropRoleList: any = [];
  dropGroupList: any = [];
  manageList: any = [];
  manageList1: any = [];
  dropdownSettings = {};
  selectedItemGrs = [];
  selectedItemRoles = [];

  ngOnInit() {
    this.callAll();
  }
  callAll(){
    

    // this.formData.groupIDs[0] = "0";
    // this.formData.roleIDs[0] = "0";
    this.formData.managerID = "0";
    // this.inputGroup=[{
    //   "id": "e6f8b263-9806-4198-1d2d-08d6f6159d78",
    //   "name": "Phòng quản lý"
    // }]
    this.loadStaffAcountService.loadGroupData().toPromise().then(data => {
      this.groupList = data;
      console.log(this.groupList);
      this.dropGroupList = [
      ];
      this.groupList.forEach(element => {
        this.dropGroupList.push({ id: element.id, name: element.name });
      });

      this.dropdownSettings = {
        singleSelection: false,
        idField: 'id',
        textField: 'name',
        selectAllText: 'Select All',
        unSelectAllText: 'UnSelect All',
        itemsShowLimit: 5,
        allowSearchFilter: true,

      };

      this.loadStaffAcountService.loadRoleData().toPromise().then(data => {
        this.roleList = data;

        this.dropRoleList = [
        ];
        this.roleList.forEach(element => {
          this.dropRoleList.push({ id: element.id, name: element.name });
        });

        this.dropdownSettings = {
          singleSelection: false,
          idField: 'id',
          textField: 'name',
          selectAllText: 'Select All',
          unSelectAllText: 'UnSelect All',
          itemsShowLimit: 5,
          allowSearchFilter: true
        };
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
              this.formData.groupIDs.forEach(element => {
                this.selectedItemGrs.push({ item_id: element.id, item_text: element.name });    
              });
              this.formData.roleIDs = this.recevieData.roles;
              this.formData.roleIDs.forEach(element => {
                this.selectedItemRoles.push({ item_id: element.id, item_text: element.name });    
              });
              this.formData.managerID = this.recevieData.managerID;

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
      let formData1 = new AccountItem(this.formData.roleIDs, this.formData.groupIDs, this.formData.managerID, this.formData.email, this.formData.password, this.formData.fullName, this.formData.dateOfBirth
        );

      let saveID: any =[];
      this.formData.groupIDs.forEach(element => {
        saveID.push(element.id);
      });
      formData1.groupIDs= [];
      formData1.groupIDs= saveID;
      let saveIDRole: any =[];
      this.formData.roleIDs.forEach(element => {
        saveIDRole.push(element.id);
      });
      formData1.roleIDs= [];
      formData1.roleIDs= saveIDRole;
      if(formData1.managerID == "0") {
        formData1.managerID = "";
      }
      this.LoginService.Register(formData1).subscribe(
        resp => {
          console.log(resp.toString());
          debugger;
          if (resp != "") {
            debugger;
            this.toastr.success('Success! ', '');
            this.dialogRef.close();
          }
          else {
            //this.errorMessage = resp.toString();    
          }
        });

    } else {
      let formData1 = new AccountItem(this.formData.roleIDs, this.formData.groupIDs, this.formData.managerID, this.formData.email, this.formData.password, this.formData.fullName, this.formData.dateOfBirth
        );

      console.log(formData1);
      let saveID: any =[];
      this.formData.groupIDs.forEach(element => {
        saveID.push(element.id);
      });
      formData1.groupIDs= [];
      formData1.groupIDs= saveID;
      let saveIDRole: any =[];
      this.formData.roleIDs.forEach(element => {
        saveIDRole.push(element.id);
      });
      formData1.roleIDs= [];
      formData1.roleIDs= saveIDRole;
      formData1.id = this.data;
      this.LoginService.editAccount(formData1).subscribe(
        resp => {
          console.log(resp.toString());
          debugger;
          if (resp != "") {
            debugger;
            this.toastr.success('Success! ', '');
            this.dialogRef.close();
          }
          else {
            //this.errorMessage = resp.toString();    
          }
        });
    }

  }

  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }
}
