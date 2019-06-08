import { Component, OnInit } from '@angular/core';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { LoginService } from 'app/service/login.service';
import { Router } from '@angular/router'; 

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
  banOrUnbanAcc(email){
    document.getElementById(email).innerHTML = "Unban";

  };
  register(){
    debugger;    
    this.LoginService.Register(this.model).subscribe(    
      data => {    
        debugger;    
        if(data !="")    
        {       
          this.router.navigate(['']);    
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

