import { Component, OnInit } from '@angular/core';    
import { Router } from '@angular/router';    
import { LoginService } from '../service/login.service';    
  
    
@Component({    
  selector: 'app-login',    
  templateUrl: './login.component.html',    
  styleUrls: ['./login.component.scss']    
})    
export class LoginComponent implements OnInit {    
    
  model : any={};      
  errorMessage:string;    

  constructor(private router:Router,private LoginService:LoginService) { }    
    
  ngOnInit() {    
    sessionStorage.removeItem('userName');    
    sessionStorage.clear();
    if(localStorage.getItem('token')!= null)
    this.router.navigate(['/dashboard']);
       
  }    
  login(){    
    debugger;    
    this.LoginService.Login(this.model).toPromise().then(    
      data => {    
        debugger;    
        if(data != "")    
        {     
          localStorage.setItem('token',data);
          this.router.navigate(['/dashboard']);
          debugger;    
        }    
        else{    
          this.errorMessage = data;    
        }    
      },    
      error => {    
        this.errorMessage = error.message;    
      });    
      
  };    
 } 
 