import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { SendRequest } from 'app/useClass/send-request';
import { ApproveRequest } from 'app/useClass/approve-request';
import { GlobalVar } from 'app/useClass/global-var';
import {MatStepperModule} from '@angular/material/stepper';



@Component({
  selector: 'app-add-handle-request',
  templateUrl: './add-handle-request.component.html',
  styleUrls: ['./add-handle-request.component.scss'],
})
export class AddHandleRequestComponent implements OnInit {

  initiatorName;
  workFlowTemplateName;
  listCmt: any = [];
  cmt;
  requestHandle: any;
  cmtHandle: any;
  requestActionHandleFile: any;
  requestActionHandleValue: any;
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
  gloUrl = GlobalVar.url;

  
  


  // formDataEdit = new AddGroupIdName();
  constructor( @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddHandleRequestComponent>, private toastr: ToastrService,
    private router: Router, private loadStaffAcountService: LoadStaffAcountService) { }

  toggleHover(event: boolean) {
    this.isHovering = event;
  }
  sendReqNextStep(nextStepID){
    var a = new String(this.cmt);
    if(a!=""){
    this.listCmt.push(a);
  }
    this.listCmt.forEach(element => {
      this.actionValues.push({ "key": "Comment" , "value": element});
    });
    debugger;
    var mdSendReq = new ApproveRequest(this.requestHandle.id, nextStepID, this.actionValues,this.data );
    console.log(JSON.stringify(mdSendReq));
    this.loadStaffAcountService.sendReqHandle(mdSendReq).toPromise().then(data =>{
      this.toastr.success('Success! ' , '' );
      this.dialogRef.close();
    },err =>{
      this.toastr.error(err.error);
    }
    )

    

  }

  closeForm(){

    this.dialogRef.close();
  }



  ngOnInit() {
    this.cmt="";
    this.requestHandle ={
      "id": "string",
      "description": "string",
      "initiatorID": "string",
      "initiatorName": "string",
      "workFlowTemplateID": "string",
      "workFlowTemplateName": "string",
      "createDate": "2019-08-14T11:55:53.918Z",
      "requestActionID": "string"
    }
    
    this.workFlowTemplateID = this.data;
    console.log(this.data);
    this.loadStaffAcountService.getHandleForm(this.workFlowTemplateID).toPromise().then(res => {
      console.log(res);
      this.saveData = res;
      console.log(this.saveData);
      this.initiatorName = this.saveData.initiatorName;
      this.workFlowTemplateName = this.saveData.workFlowTemplateName;
      this.buttons = this.saveData.connections;
      this.requestHandle = this.saveData.request;
      this.requestActionHandleFile = this.saveData.userRequestAction.requestFiles;
      this.requestActionHandleFile.forEach(element => {
        element.path = this.gloUrl +element.path;
        let a ="\\";
        console.log("here:", a);
        // a.lastIndexOf()
        element.name = element.path.substring(element.path.lastIndexOf('\\')+1);

      });

      this.requestActionHandleValue = this.saveData.userRequestAction.requestValues;
      this.cmtHandle = this.saveData.staffRequestActions;

      console.log("cmtHandle", this.cmtHandle);
      
    },err =>{
      this.toastr.error(err.error);
    })

  }
  addNewCmt(){
    var a = new String(this.cmt);

    this.listCmt.push(a);
    this.cmt = "";
  }



 
  

}
