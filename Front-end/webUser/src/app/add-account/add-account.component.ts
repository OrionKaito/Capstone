import { Component, OnInit, Inject } from '@angular/core';
import { AccountItem } from 'app/useClass/account-item';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { Router } from '@angular/router';
import { inject } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { LoginService } from 'app/service/login.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-account',
  templateUrl: './add-account.component.html',
  styleUrls: ['./add-account.component.scss']
})
export class AddAccountComponent implements OnInit {
  formData = new AccountItem;
  dataGet: any = [];
  recevieData: any;


  createAcc = true;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddAccountComponent>,
    private router: Router, private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService, private toastr: ToastrService) { }
  permissionList: any = [];

  groupList: any = [];

  ngOnInit() {
    this.formData.permissionToEditID = "0";
    this.formData.permissionToUseID = "0";

    if (this.data != null && this.data != "null") this.createAcc = false;
    this.loadStaffAcountService.loadPermissionData().toPromise().then(data => {
      this.permissionList = data;
      if (!this.createAcc) {
        this.loadStaffAcountService.loadWFByID(this.data).toPromise().then(data => {
  
          this.recevieData = data;
          this.formData.name = this.recevieData.name;
          this.formData.id = this.recevieData.id;
          this.formData.data = this.recevieData.data;
          this.formData.description = this.recevieData.description;
        //  this.formData.description = "Ádasd";
       
          this.formData.permissionToUseID = this.recevieData.permissionToUseID;
          console.log(this.formData);
        })
      }
    })
   



  }
  onSubmit() {

    if(this.createAcc){
    this.LoginService.addNewWF(this.formData).toPromise().then(
      resp => {
        if (resp != "") {
          debugger;
          this.toastr.success('Success! ', '');
          this.dialogRef.close();
        }
        else {
          //this.errorMessage = resp.toString();    
        }
      }, (err) =>{
        this.toastr.error("Please try it again!", "Something wrong");
      });

    } else {
      this.LoginService.editWF(this.formData).toPromise().then(
        resp => {
          if (resp != "") {
            debugger;
            this.toastr.success('Success! ', '');
            this.dialogRef.close();
          }
          else {
            //this.errorMessage = resp.toString();    
          }
        }, (err) =>{
          this.toastr.error("Please try it again!", "Something wrong");
        });
    }

  }

}
