import { Component, OnInit } from '@angular/core';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manage-group',
  templateUrl: './manage-group.component.html',
  styleUrls: ['./manage-group.component.scss']
})
export class ManageGroupComponent implements OnInit {
  groupGets: any= [];
  value = '';
  model : any={}; 
  errorMessage:string; 
  constructor(private router:Router, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() { 
    this.loadStaffAcountService.loadGroupData().subscribe(data => {
      console.log(data);
      this.groupGets = data;
    })
  }
  deleteGroup(id){
    
    debugger;
     this.loadStaffAcountService.deleteGroup(id).subscribe(
       data=>{
         console.log(data);   
              
       },
       err=>{
       console.log(err);
     }
       )
       location.reload(); 
   
 };
 
}
