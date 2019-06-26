import { Component, OnInit, Inject } from '@angular/core';
import { AddGroup } from 'app/useClass/add-group';
import { AddGroupIdName } from 'app/useClass/add-group-id-name';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-add-permission',
  templateUrl: './add-permission.component.html',
  styleUrls: ['./add-permission.component.scss']
})
export class AddPermissionComponent implements OnInit {

  createGroup = true; 
  formData = new AddGroup();
  saveData: any;
  formDataEdit = new AddGroupIdName();
  constructor(@Inject(MAT_DIALOG_DATA) public data,
  public dialogRef: MatDialogRef<AddPermissionComponent>, private toastr: ToastrService,
  private router:Router, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() {
    if(this.data != null && this.data != "null") this.createGroup =false;
    if(!this.createGroup){
      this.loadStaffAcountService.loadPermissionByID(this.data).toPromise().then(res => {
        this.saveData = res;
       this.formData.name =this.saveData.name;
      })
    } 
  }
  onSubmit(){
    if(this.createGroup){
      this.loadStaffAcountService.addPermission(this.formData).toPromise().then(    
        resp => {    
          console.log(resp.toString())   ;  
         
        });    
      
    } else {
      this.formDataEdit.name =this.formData.name;
      this.formDataEdit.id =this.data;
      this.loadStaffAcountService.editPermission(this.formDataEdit).toPromise().then(    
        resp => {    
          console.log(resp.toString())   ;
          debugger;   
       
          if(resp !="")    
          {                   
            debugger;          
          }    
          else{    
            //this.errorMessage = resp.toString();    
          }    
        }); 
    }
    this.toastr.success('Success! ' , '' );
    this.dialogRef.close();
  }



}
