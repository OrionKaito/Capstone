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
userDetail1;
  constructor(private router: Router , private loginService: LoginService ) { }

  ngOnInit() {
    debugger;
    this.loginService.getUserProfile().toPromise().then(
      resp =>{
        debugger;
        this.userDetail1 = resp;
        this.userDetail = this.userDetail1[0];
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
