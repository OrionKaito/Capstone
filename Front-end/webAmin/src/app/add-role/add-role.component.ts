import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { AddRole } from 'app/useClass/add-role';
import { AddRoleIdName } from 'app/useClass/add-role-id-name';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-add-role',
  templateUrl: './add-role.component.html',
  styleUrls: ['./add-role.component.scss']
})
export class AddRoleComponent implements OnInit {
  createRole = true; 
  formData = new AddRole();
  saveData: any;
  formDataEdit = new AddRoleIdName();
  constructor(@Inject(MAT_DIALOG_DATA) public data,
  public dialogRef: MatDialogRef<AddRoleComponent>, private toastr: ToastrService,
  private router:Router, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() {
    
    if(this.data != null && this.data != "null") this.createRole =false;
    if(!this.createRole){
      this.loadStaffAcountService.loadRoleByID(this.data).toPromise().then(res => {
        this.saveData = res;
       this.formData.name =this.saveData.name;
      })
    } 
  }
  onSubmit(){
    if(this.createRole){
      
      this.loadStaffAcountService.addRole(this.formData).toPromise().then(    
        resp => {    
          console.log(resp.toString())   ;           
        });    

        // this.dialogRef.close();

    } else {
      this.formDataEdit.name =this.formData.name;
      this.formDataEdit.id =this.data;
      this.loadStaffAcountService.editRole(this.formDataEdit).subscribe(    
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

        // this.dialogRef.close();    
    }
    this.toastr.success('Success! ' , '' );
    this.dialogRef.close();
  }

}
