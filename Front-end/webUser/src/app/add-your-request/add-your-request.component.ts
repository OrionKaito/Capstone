import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { AddPermissionComponent } from 'app/add-permission/add-permission.component';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-add-your-request',
  templateUrl: './add-your-request.component.html',
  styleUrls: ['./add-your-request.component.scss']
})
export class AddYourRequestComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data,
  public dialogRef: MatDialogRef<AddPermissionComponent>, private toastr: ToastrService,
  private router: Router, private loadStaffAcountService: LoadStaffAcountService) { }

  yourRequest: any;

  ngOnInit() {
    this.yourRequest ={
      workFlowTemplateName: "",
      status: "",
      staffResult: []
    }
    console.log(this.data);
    this.loadStaffAcountService.getYourRequest(this.data.id).toPromise().then(res=>{
      this.yourRequest = res;
      console.log(this.yourRequest);
    },err =>{
      this.toastr.error(err.error);
    })
  }
  closeForm(){
    debugger;
    this.dialogRef.close();
  }
}
