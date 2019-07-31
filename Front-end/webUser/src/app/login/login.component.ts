


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
    email: string;
    formname: string;
    title: string;
    checkPage: boolean;
    nameOfbtn: string;
    nameOfsummit: string;
    showCode: boolean;
    ngsummitFun: string;



    resetPassmodel: any;
    constructor(private router: Router, private LoginService: LoginService, private loadStaffAcountService: LoadStaffAcountService) { }

    ngOnInit() {
        this.loadStaffAcountService.getPermission();
        this.loadStaffAcountService.receiveMessage();
        this.message = this.loadStaffAcountService.currentMessage;
        this.formname = 'loginForm';
        this.title = 'Sign In';
        this.nameOfbtn = 'Forget Password';
        this.checkPage = true;
        this.showCode = false;
        this.nameOfsummit = 'Sign In';
        this.ngsummitFun = 'forgetPass()';
        sessionStorage.removeItem('userName');
        sessionStorage.clear();
        let haveRole = false;
        this.loadStaffAcountService.checkRole().toPromise().then(res => {
            haveRole = true;

            let save = false;
            if (localStorage.getItem('saveMe') == "true") {
                save = true;
            }
            if (haveRole && save) this.router.navigate(['/dashboard']);
        }, err => {

        });
    }
    //   login() {
    //     debugger;
    //     let tokenNoti = localStorage.getItem("tokenNoti");
    //     if(tokenNoti == null){tokenNoti = ""};
    //     let loginWithTkNoti ={userName: this.model.Username, password: this.model.Password, deviceID:  tokenNoti};
    //     this.LoginService.Loginv2(loginWithTkNoti).toPromise().then(
    //       // this.LoginService.Login(this.model).toPromise().then(
    //       data => {
    //         if (data.status == 200) {
    //           this.dataNow = data.body;

    //             var a = this.dataNow.token;
    //             debugger;

    //             localStorage.setItem('token', a);
    //             this.router.navigate(['/dashboard']);
    //         }
    //         this.resetPassmodel = {
    //             email: '',
    //             code: '',
    //             password: ''
    //         }
    //     }

    login() {
        let tokenNoti = localStorage.getItem("tokenNoti");
        if (tokenNoti == null) { tokenNoti = "" };
        let loginWithTkNoti = { userName: this.model.Username, password: this.model.Password, deviceToken: tokenNoti };
        debugger;
        console.log(loginWithTkNoti);
        this.LoginService.Login(loginWithTkNoti).toPromise().then(
            data => {
                if (data.status == 200) {
                    this.dataNow = data.body;

                    var a = this.dataNow.token;
                    debugger;

                    localStorage.setItem('token', a);
                    if (this.saveMe) {
                        localStorage.setItem('saveMe', "true");
                    }
                    else {
                        localStorage.setItem('saveMe', "false");
                    }
                    this.router.navigate(['/dashboard']);
                    debugger;
                } else if (0) {
                    this.dataNow = data.body;
                    if (this.dataNow == 'Invalid username or password!') {
                        this.errorMessage = 'Invalid username or password!';
                    } else if (this.dataNow == 'Please verify your account first!') {

                    } else if (this.dataNow == 'Account is banned!') {
                        this.errorMessage = 'Account is banned!';
                    }

                }

            },
            error => {
                this.errorMessage = 'Invalid username or password!';
            });

    };

    forgetPass() {
        this.checkPage = false;
        this.LoginService.resetPassword(this.email).toPromise().then(
            data => {
                this.showCode = true;
                this.nameOfsummit = 'Send code';
                this.ngsummitFun = '';
                this.errorMessage = data;
                this.title = 'Change Password';
            },
            err => {
                this.errorMessage = 'Email Incorret';
                this.showCode = false;
                this.ngsummitFun = 'forgetPass()';
            }
        )
    };

    resetPass() {
        this.errorMessage = '';
        this.LoginService.sendCodeConfig(this.resetPassmodel.code, this.resetPassmodel.email, this.resetPassmodel.password).toPromise().then(
            data => {
                this.errorMessage = 'change Password' + data;
                this.checkPage = true;
                this.showCode = false;
                console.log(data);
            },
            err => {
                this.errorMessage = 'Error code'
                console.log(this.errorMessage);
            }
        )
    }

    changePage() {
        if (this.checkPage) {
            this.nameOfbtn = 'Sign In';
            this.checkPage = false;
            this.nameOfsummit = 'Send Email';
            this.errorMessage = '';
            this.title = 'Send Email';
        } else {
            this.checkPage = true;
            this.nameOfbtn = 'Forget Password';
            this.nameOfsummit = 'Sign In';
            this.errorMessage = '';
            this.title = 'Sign In';
        }
    }

}
