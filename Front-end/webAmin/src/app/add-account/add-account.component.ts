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
     this.formData.roleID = "0";
    this.formData.LineManagerID = "0";
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

        // this.dropRoleList = [
        // ];
        // this.roleList.forEach(element => {
        //   this.dropRoleList.push({ id: element.id, name: element.name });
        // });

        // this.dropdownSettings = {
        //   singleSelection: false,
        //   idField: 'id',
        //   textField: 'name',
        //   selectAllText: 'Select All',
        //   unSelectAllText: 'UnSelect All',
        //   itemsShowLimit: 5,
        //   allowSearchFilter: true
        // };
        this.loadStaffAcountService.loadStaffData().toPromise().then(data => {
          this.manageList1 = data;
          this.manageList = this.manageList1.accounts;
          let index =0;
          let same;
          this.manageList.forEach(element => {
            if(element.id== this.data) {
              // let a = [];
             same = index;
            }
            index = index+1;
          });
          this.manageList.splice(same,1);
          console.log(this.manageList);
          if (this.data != null && this.data != "null") this.createAcc = false;
          if (!this.createAcc) {

            this.loadStaffAcountService.loadUserByID(this.data).toPromise().then(data => {
              
              this.recevieData1 = data;

              this.recevieData = this.recevieData1[0];
              console.log(this.recevieData);
              this.formData.email = this.recevieData.email;
              this.formData.dateOfBirth = this.recevieData.dateOfBirth;
              this.formData.fullName = this.recevieData.fullName;
              //this.formData.roleID = this.recevieData.roleID;
              this.formData.groupIDs = this.recevieData.groups;
              this.formData.groupIDs.forEach(element => {
                this.selectedItemGrs.push({ item_id: element.id, item_text: element.name });    
              });
              this.formData.roleID = this.recevieData.role.id;
              
              // this.formData.roleID.forEach(element => {
              //   this.selectedItemRoles.push({ item_id: element.id, item_text: element.name });    
              // });
              if( this.recevieData.lineManagerID != null){
                this.formData.LineManagerID = this.recevieData.lineManagerID;
              }
             

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
    
      let formData1;
      formData1 = new AccountItem(this.formData.roleID,
         this.formData.groupIDs, 
         this.formData.LineManagerID, 
         this.formData.email, 
         this.formData.password, 
         this.formData.fullName, 
         this.formData.dateOfBirth
        );

      let saveID: any =[];
      this.formData.groupIDs.forEach(element => {
        saveID.push(element.id);
      });
      formData1.groupIDs= [];
      formData1.groupIDs= saveID;
     // let saveIDRole: any =[];
      // this.formData.roleIDs.forEach(element => {
      //   saveIDRole.push(element.id);
      // });
  
      formData1.roleID= this.formData.roleID;
      let model: any;
      if(formData1.LineManagerID == "0") {
        model={
          groupIDs: formData1.groupIDs,
          // LineManagerID: formData1.LineManagerID,
          email: formData1.email,
          password: formData1.password,
          fullName: formData1.fullName,
          dateOfBirth: formData1.dateOfBirth,
          roleID: formData1.roleID
        }
      } else{
        model = formData1;
      }
      // console.log(JSON.stringify(model));
      this.LoginService.Register(model).subscribe(
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
        }, err=>  {
          this.toastr.error(err.error);
        });

    } else {
      debugger;
      let formData1 = new AccountItem(this.formData.roleID, this.formData.groupIDs, this.formData.LineManagerID, this.formData.email, this.formData.password, this.formData.fullName, this.formData.dateOfBirth
        );

      console.log(formData1);
      let saveID: any =[];
      this.formData.groupIDs.forEach(element => {
        saveID.push(element.id);
      });
      formData1.groupIDs= [];
      formData1.groupIDs= saveID;
     // let saveIDRole: any =[];
      // this.formData.roleID.forEach(element => {
      //   saveIDRole.push(element.id);
      // });
      formData1.roleID= this.formData.roleID;
      formData1.id = this.data;
      let model: any;
      if(formData1.LineManagerID == "0") {
        model={
          id: this.data,
          groupIDs: formData1.groupIDs,
          // LineManagerID: formData1.LineManagerID,
          email: formData1.email,
          password: formData1.password,
          fullName: formData1.fullName,
          dateOfBirth: formData1.dateOfBirth,
          roleID: formData1.roleID
        }
      } else{
        model = formData1;
      }
      console.log("in ra đây:", model);
      console.log(JSON.stringify(model));

      this.LoginService.editAccount(model).subscribe(
        resp => {
          if (resp != "") {
            this.toastr.success('Success! ', '');
            this.dialogRef.close();
          }
          else {
            //this.errorMessage = resp.toString();    
          }
        },err=>  {
          this.toastr.error(err.error);
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
