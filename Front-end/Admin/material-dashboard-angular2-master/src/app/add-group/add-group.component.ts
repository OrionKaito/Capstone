import { Component, OnInit, Inject } from '@angular/core';
import { AddGroup } from 'app/useClass/add-group';
import { AddGroupIdName } from 'app/useClass/add-group-id-name';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';

@Component({
  selector: 'app-add-group',
  templateUrl: './add-group.component.html',
  styleUrls: ['./add-group.component.scss']
})
export class AddGroupComponent implements OnInit {

  createGroup = true; 
  formData = new AddGroup();
  saveData: any;
  formDataEdit = new AddGroupIdName();
  constructor(@Inject(MAT_DIALOG_DATA) public data,
  public dialogRef: MatDialogRef<AddGroupComponent>,
  private router:Router, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() {
    if(this.data != null && this.data != "null") this.createGroup =false;
    if(!this.createGroup){
      this.loadStaffAcountService.loadGroupByID(this.data).toPromise().then(res => {
        this.saveData = res;
       this.formData.name =this.saveData.name;
      })
    } 
  }
  onSubmit(){
    if(this.createGroup){
      this.loadStaffAcountService.addGroup(this.formData).toPromise().then(    
        resp => {    
          console.log(resp.toString())   ;  
          location.reload();
        });    
        
    } else {
      this.formDataEdit.name =this.formData.name;
      this.formDataEdit.id =this.data;
      this.loadStaffAcountService.editGroup(this.formDataEdit).toPromise().then(    
        resp => {    
          console.log(resp.toString())   ;
          debugger;   
          location.reload();
          if(resp !="")    
          {                   
            debugger;          
          }    
          else{    
            //this.errorMessage = resp.toString();    
          }    
        }); 
    }
  }


}
