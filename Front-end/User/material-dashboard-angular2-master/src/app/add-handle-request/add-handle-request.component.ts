import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';

@Component({
  selector: 'app-add-handle-request',
  templateUrl: './add-handle-request.component.html',
  styleUrls: ['./add-handle-request.component.scss']
})
export class AddHandleRequestComponent implements OnInit {

  saveData: any;
  buttons: any = [];
  formKey: any;
  formValue: any;
  checkLoadFile = 0;
  listURL: any = [];
  isHovering: boolean;
  value: any
  downloadURL: string;
  files: File[] = [];
  actionValues: any = [];
  workFlowTemplateID: any;
  // formDataEdit = new AddGroupIdName();
  constructor( @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddHandleRequestComponent>, private toastr: ToastrService,
    private router: Router, private loadStaffAcountService: LoadStaffAcountService) { }

  toggleHover(event: boolean) {
    this.isHovering = event;
  }
  sendReqNextStep(nextStepID){
    // this.actionValues.push({ "key": this.formKey, "value": this.formValue})
    // var mdSendReq = new SendRequest("", this.actionValues, this.listURL, this.workFlowTemplateID, nextStepID);
    // this.loadStaffAcountService.sendReq(mdSendReq).toPromise().then(data =>{
    //   this.toastr.success('Success! ' , '' );
    //   console.log(this.downloadURL);
    //   console.log("aaaa");
    //   console.log(this.listURL);
    //   this.dialogRef.close();
    // }
    // )
  }




  ngOnInit() {
    debugger;
    this.workFlowTemplateID = this.data;
    this.loadStaffAcountService.getHandleForm(this.workFlowTemplateID).toPromise().then(res => {
      this.saveData = res;
      this.buttons = this.saveData.connections;
    })

  }


}
