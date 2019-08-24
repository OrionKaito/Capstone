import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {LoginService} from '../service/login.service';
import {ROUTES} from 'app/components/sidebar/sidebar.component';
import {LoadStaffAcountService} from 'app/service/load-staff-acount.service';
import {ToastrService} from 'ngx-toastr';
import Swal from 'sweetalert2';


@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit {
    m = false;
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


    resetPassmodel: any = {};

    constructor(private toastr: ToastrService, private router: Router, private LoginService: LoginService, private loadStaffAcountService: LoadStaffAcountService) {
    }

    ngOnInit() {

        this.loadStaffAcountService.getPermission();
        this.loadStaffAcountService.receiveMessage();
        this.message = this.loadStaffAcountService.currentMessage;

        this.formname = 'loginForm';
        this.title = 'Sign In';
        this.nameOfbtn = 'Forgot Password';
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
                if (localStorage.getItem('saveMe') == 'true') {
                    save = true;
                }
                if (haveRole && save) {
                    this.router.navigate(['/create-request']);
                }
            }
            // ,err =>{
            //     this.toastr.error(err.error);
            //   }
        );
    }


    login() {
        let tokenNoti = localStorage.getItem('tokenNoti');
        if (tokenNoti == null) {
            tokenNoti = ''
        }
        ;
        let loginWithTkNoti = {userName: this.model.Username, password: this.model.Password, deviceToken: tokenNoti};
        debugger;
        this.LoginService.Login(loginWithTkNoti).toPromise().then(
            data => {
                if (data.status == 200) {
                    this.dataNow = data.body;

                    var a = this.dataNow.token;
                    debugger;

                    localStorage.setItem('token', a);
                    if (this.saveMe) {
                        localStorage.setItem('saveMe', 'true');
                    } else {
                        localStorage.setItem('saveMe', 'false');
                    }
                    this.router.navigate(['/create-request']);
                 
                    this.LoginService.getUserProfile().toPromise().then(
                        resp =>{
                            let data: any;
                            data = resp;
                            localStorage.setItem("name", data[0].fullName);
                        },err =>{
                          this.toastr.error(err.error);
                        }
                      )
                }
            }, err => {


                let errNow = err.error;
                if (errNow.startsWith('Invali')) {
                    this.toastr.error(err.error);
                } else if (errNow.startsWith('Please verify your')) {
                    this.confirmEmail();

                } else if (errNow.startsWith('Account is banned')) {
                    Swal.fire({
                        type: 'error',
                        title: 'Your account had been baned',
                        text: 'Sorry, your account had been baned, please contact with your line manage to know the issue! ',
                        footer: 'We apologize for this inconvenience.'
                    })
                }


            });

    };

    forgetPass() {
        this.checkPage = false;
        this.LoginService.resetPassword(this.email).toPromise().then(
            data => {
                this.showCode = true;
                this.nameOfsummit = 'Send code';
                this.ngsummitFun = '';
                this.toastr.success(data);
                this.title = 'Change Password';
            },
            err => {
                this.toastr.error("You entered the wrong email");
                this.showCode = false;
                this.ngsummitFun = 'forgetPass()';
            }
        )
    };

    resetPass() {

        this.errorMessage = '';
        this.LoginService.sendCodeConfig(this.resetPassmodel.code, this.email, this.resetPassmodel.password).toPromise().then(
            data => {
                this.checkPage = true;
                this.showCode = false;
                this.toastr.success(data + '!!');
                this.title = 'Sign In';
                this.nameOfsummit = 'Sign In';
            },
            err => {
                this.toastr.error('Incorrect Email!!');
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
            this.nameOfbtn = 'Forgot Password';
            this.nameOfsummit = 'Sign In';
            this.errorMessage = '';
            this.title = 'Sign In';
        }
    }

    async confirmEmail() {

        const {value: formValues} = await Swal.fire({
            title: 'Please verify your account first:',
            html:
                '<input id="swal-input1" placeholder="Your email" class="swal2-input">' +
                '<input id="swal-input2"  placeholder="Code" class="swal2-input">',
            focusConfirm: true,
            showLoaderOnConfirm: true,
            showCloseButton: true,
            preConfirm: () => {
                let a = {
                    email: (<HTMLInputElement>document.getElementById('swal-input1')).value.toString(),
                    code: (<HTMLInputElement>document.getElementById('swal-input2')).value.toString()
                }
                this.loadStaffAcountService.validateAcc(a).toPromise().then(res => {
                    this.toastr.success('Success!!');
                    this.m = true;
                    return true;
                }, err => {
                    this.m = false;
                    this.toastr.error(err.error);

                })
            }
        })
    }
}
