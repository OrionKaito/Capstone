import { Component, OnInit } from '@angular/core';    
import { Router } from '@angular/router';    

 import { FormsModule } from '@angular/forms';    
import { from } from 'rxjs';
import { LoginService } from './login.service';
    
@Component({    
  selector: 'app-login',    
  templateUrl: './login.component.html',    
  styleUrls: ['./login.component.css']    
})    
export class LoginComponent {    
    
  model : any={};    
    
  errorMessage:string;    
  constructor(private router:Router,private LoginService:LoginService) { }    
    
    
  ngOnInit() {    
    sessionStorage.removeItem('UserName');    
    sessionStorage.clear();    
  }    
  login(){    
    debugger;    
    this.LoginService.Login(this.model).subscribe(    
      data => {    
        debugger;    
        if(data.toString() != "")    
        {       
          this.router.navigate(['/create-request']);    
          debugger;    
        }    
        else{    
          this.errorMessage = data.toString();    
        }    
      },    
      error => {    
        this.errorMessage = error.error;    
      });    
  };    
 } 
