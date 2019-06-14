import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { element } from '@angular/core/src/render3';

@Component({
  selector: 'app-manage-role',
  templateUrl: './manage-role.component.html',
  styleUrls: ['./manage-role.component.scss']
})
export class ManageRoleComponent implements OnInit {
  roleGets: any= [];
  value = '';
  model : any={}; 
  errorMessage:string; 
  
  constructor(private router:Router, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() {
    this.loadStaffAcountService.loadRoleData().subscribe(data => {
      console.log(data);
      this.roleGets = data;
    })
  }
  deleteRole(id){
    
     debugger;
      this.loadStaffAcountService.deleteRole(id).subscribe(
        data=>{
          console.log(data);   
               
        },
        err=>{
        console.log(err);
      }
        )
        location.reload(); 
    
  };
  addRole(){ 
      this.loadStaffAcountService.addRole(this.model).subscribe(    
        resp => {    
          console.log(resp.toString());
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
