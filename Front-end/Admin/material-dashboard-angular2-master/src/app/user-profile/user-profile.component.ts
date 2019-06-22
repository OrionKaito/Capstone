import { Component, OnInit } from '@angular/core';
import { Route } from '@angular/compiler/src/core';
import { LoginService } from 'app/service/login.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
userDetail;
  constructor(private router: Router , private loginService: LoginService ) { }

  ngOnInit() {
    debugger;
    this.loginService.getUserProfile().subscribe(
      resp =>{
        debugger;
        this.userDetail = resp;
        console.log("test thư:" + resp.toString());
      },
      err => {
        console.log(err);
      }
    )
  }
  updateProfile(){

    
    this.loginService.updateUserProfile(this.userDetail.fullName,this.userDetail.dateOfBirth).subscribe(
      data=>{
        console.log(data);       
      },
      err=>{
        console.log(err);
      }

    )
  }

}
