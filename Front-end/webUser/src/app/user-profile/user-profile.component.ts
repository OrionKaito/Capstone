import { Component, OnInit } from '@angular/core';
import { Route } from '@angular/compiler/src/core';
import { LoginService } from 'app/service/login.service';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
userDetail1: any=[];
userDetail;
changePass: any;

  constructor(private router: Router ,private toastr: ToastrService, private loginService: LoginService ) { }

  ngOnInit() {
    this.changePass= {
      cur : "",
      new: "",
      re: ""
    };
    debugger;
    this.loginService.getUserProfile().toPromise().then(
      resp =>{
        debugger;
        this.userDetail1 = resp;
        this.userDetail = this.userDetail1[0];
        
        // this.userDetail.dateOfBirth = Date.parse(this.userDetail.dateOfBirth) ;
        // console.log(this.userDetail.dateOfBirth);
      },
      err => {
        console.log(err);
      }
    )
  }
  updateProfile(){
    this.loginService.updateUserProfile(this.userDetail.fullName,this.userDetail.dateOfBirth).subscribe(
      data=>{
        this.toastr.success("Update your profile success!!!", "Success!");   
      },
      err=>{
        this.toastr.error("Error: " + err.message + ". Please do it later!!", "Something wrong!");   
      }

    )
  }

  changePassword(){
    debugger;
    this.loginService.changePass(this.changePass.cur, this.changePass.new).subscribe(
      data=>{
        this.toastr.success("Change your password success!!!", "Success!");   
      },
      err=>{
        this.toastr.error("Error: " + err.message + ". Please try again!!", "Something wrong!");   
      }
    )
  }

}
