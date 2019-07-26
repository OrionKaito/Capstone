import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../service/login.service';
import { ROUTES } from 'app/components/sidebar/sidebar.component';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  message: any;
  model: any = {};
  wrongPass: any;
  dataNow: any = {};
  errorMessage: any;
  saveMe: any;

  constructor(private router: Router, private LoginService: LoginService, private loadStaffAcountService: LoadStaffAcountService ) { }

  ngOnInit() {
    this.loadStaffAcountService.getPermission();
    this.loadStaffAcountService.receiveMessage();
    this.message = this.loadStaffAcountService.currentMessage;
  
    sessionStorage.removeItem('userName');
    sessionStorage.clear();
    if (localStorage.getItem('token') != null && localStorage.getItem('saveMe') != null)
      this.router.navigate(['/dashboard']);

  }
  login() {
    debugger;
    let tokenNoti = localStorage.getItem("tokenNoti");
    if(tokenNoti == null){tokenNoti = ""};
    let loginWithTkNoti ={userName: this.model.Username, password: this.model.Password, deviceID:  tokenNoti};
    this.LoginService.Loginv2(loginWithTkNoti).toPromise().then(
      // this.LoginService.Login(this.model).toPromise().then(
      data => {
        if (data.status == 200) {
          this.dataNow = data.body;
          
            var a = this.dataNow.token;
            debugger;

            localStorage.setItem('token', a);
            this.router.navigate(['/dashboard']);
            debugger;
          } else if (0) {
            this.dataNow = data.body;
            if (this.dataNow == "Invalid username or password!") {
              this.wrongPass = "Invalid username or password!";
            } else if (this.dataNow == "Please verify your account first!") {

            } else if (this.dataNow == "Account is banned!") {
              this.wrongPass = "Account is banned!";
            }

          }

        },
        error => {
          this.errorMessage = error.message;
        });

  };
}
