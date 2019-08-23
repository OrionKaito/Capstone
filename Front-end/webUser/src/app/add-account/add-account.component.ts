import { Component, OnInit, Inject } from '@angular/core';
import { AccountItem } from 'app/useClass/account-item';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { Router } from '@angular/router';
import { inject } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { LoginService } from 'app/service/login.service';
import { ToastrService } from 'ngx-toastr';
import {FormControl, Validators} from '@angular/forms';

@Component({
  selector: 'app-add-account',
  templateUrl: './add-account.component.html',
  styleUrls: ['./add-account.component.scss']
})
export class AddAccountComponent implements OnInit {

  listIcon=["folder_special", "alarm", "account_balance_wallet",
            "account_balance", "calendar_today", "commute", "library_books", 
            "videocam", "airplanemode_active" ];
  formData = new AccountItem;
  dataGet: any = [];
  recevieData: any;


  createAcc = true;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddAccountComponent>,
    private router: Router,
    private loadStaffAcountService: LoadStaffAcountService,
    private LoginService: LoginService, private toastr: ToastrService) { }
  permissionList: any = [];

  groupList: any = [];

  ngOnInit() {
    this.chooseIcon("folder_special");
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
          this.formData.isViewDetail = this.recevieData.isViewDetail;
          this.formData.icon = this.recevieData.icon;

          //  this.formData.description = "Ádasd";

          this.formData.permissionToUseID = this.recevieData.permissionToUseID;
          console.log(this.formData);
        }, err =>{
          this.toastr.error(err.error);
        })
      }
    },err =>{
      this.toastr.error(err.error);
    })




  }
  onSubmit() {
    if (this.formData.name == "" ||
      this.formData.description == "" ||
      this.formData.permissionToUseID == "" ||
      this.formData.permissionToUseID == "0") {
      this.toastr.error("Please fill all fields!");
    } else {
      if (this.createAcc) {
        console.log(this.formData);
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
          }, (err) => {
            this.toastr.error(err.error);
          });

      } else {
        console.log("form",this.formData);
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
          }, (err) => {
            this.toastr.error(err.error);
          });
      }

    }
  }
  emailFormControl = new FormControl('', [
    Validators.required
  ]);
  descriptionFormControl = new FormControl('', [
    Validators.required
  ]);
  chooseIcon(item){
    this.listIcon.forEach(element => {
      $('#icon'+element).removeClass("borderGreen");
    });
    $('#icon'+item).addClass("borderGreen");
    this.formData.icon=item.toString();
  }
}
