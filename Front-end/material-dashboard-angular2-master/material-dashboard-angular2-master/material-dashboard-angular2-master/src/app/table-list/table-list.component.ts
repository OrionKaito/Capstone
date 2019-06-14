import { Component, OnInit } from '@angular/core';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { LoginService } from 'app/service/login.service';
import { Router } from '@angular/router'; 
import { getLocaleDateTimeFormat } from '@angular/common';

@Component({
  selector: 'app-table-list',
  templateUrl: './table-list.component.html',
  styleUrls: ['./table-list.component.css']
})
export class TableListComponent implements OnInit {
  users: any= [];
  value = '';
  model : any={}; 
  errorMessage:string; 
  constructor(private router:Router, private loadStaffAcountService: LoadStaffAcountService, private LoginService : LoginService) { }

  ngOnInit() {
 
    this.loadStaffAcountService.loadStaffData().subscribe(data => {
      console.log(data);

      this.users = data;
    })
  }
  banOrUnbanAcc(id){

    if(document.getElementById(id).innerHTML == "Ban") {
      document.getElementById(id).innerHTML = "Unban";
      this.LoginService.BanOrUnbanAcc(id).subscribe(
        data=>{
          console.log(data);
          document.getElementById(id).innerHTML = "Unban";
        },
        err=>{
        console.log(err);
      }
        )
    
    }else {
      document.getElementById(id).innerHTML = "Ban";
      this.LoginService.BanOrUnbanAcc(id).subscribe(
        data=>{
          console.log(data);
          document.getElementById(id).innerHTML = "Ban";
        },
        err=>{
          console.log(err);
        }
        )  
    }
  };
  register(){
  this.model.dateOfBirth  = this.model.dateOfBirth.toString() + "T06:08:08-05:00";
    this.LoginService.Register(this.model).subscribe(    
      resp => {    
        console.log(resp.toString())   ;
        debugger; 
        
        if(resp !="")    
        {                   
          debugger;    
          location.reload();
        }    
        else{    
          this.errorMessage = resp.toString();    
        }    
      },    
      error => {    
        this.errorMessage = error.message;    
      }); 
     

  };
}

