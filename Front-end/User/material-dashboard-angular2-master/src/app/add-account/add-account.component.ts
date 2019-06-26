import { Component, OnInit, Inject } from '@angular/core';
import { AccountItem } from 'app/useClass/account-item';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { Router } from '@angular/router';
import { inject } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { LoginService } from 'app/service/login.service';

@Component({
  selector: 'app-add-account',
  templateUrl: './add-account.component.html',
  styleUrls: ['./add-account.component.scss']
})
export class AddAccountComponent implements OnInit {
  formData = new AccountItem;
  dataGet: any =[];
  recevieData: any


  createAcc = true; 
  constructor(
    @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddAccountComponent>,
    private router:Router, private loadStaffAcountService: LoadStaffAcountService, private LoginService: LoginService) { }
  permissionList: any=[]
  groupList: any=[]

  ngOnInit() {
    

    if(this.data != null && this.data != "null") this.createAcc =false;
    if(!this.createAcc){
      this.loadStaffAcountService.loadWFByID(this.data).toPromise().then(data => {
        this.recevieData = data;
      })
    }
    this.loadStaffAcountService.loadPermissionData().toPromise().then(data => {
      this.permissionList = data;
      console.log("per:");
      console.log(this.permissionList);
    })
    this.formData.permission="0";
  }
  onSubmit(){
    if(this.createAcc){
      //this.formData.dateOfBirth  = this.formData.dateOfBirth.toString() + "T06:08:08-05:00";
      this.LoginService.Register(this.formData).subscribe(    
        resp => {    
          console.log(resp.toString())   ;
          debugger;       
          if(resp !="")    
          {                   
            debugger;          
          }    
          else{    
            //this.errorMessage = resp.toString();    
          }    
        });    
        
    } else {
      this.LoginService.Register(this.formData).subscribe(    
        resp => {    
          console.log(resp.toString())   ;
          debugger;       
          if(resp !="")    
          {                   
            debugger;          
          }    
          else{    
            //this.errorMessage = resp.toString();    
          }    
        }); 
    }
  }

}
