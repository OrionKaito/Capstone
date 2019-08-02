import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { AngularFireStorage, AngularFireUploadTask } from '@angular/fire/storage';
import { AngularFirestore } from '@angular/fire/firestore';
import { Observable } from 'rxjs';
import { tap, finalize } from 'rxjs/operators';
import { SendRequest } from 'app/useClass/send-request';

@Component({
  selector: 'app-add-new-request',
  templateUrl: './add-new-request.component.html',
  styleUrls: ['./add-new-request.component.scss']
})
export class AddNewRequestComponent implements OnInit {

  // formData = new AddGroup();
  saveData: any;
  buttons: any = [];
  formKey: any;
  formValue: any;
  checkLoadFile = 0;
  listURL: any = [];
  isHovering: boolean;
  value: any
  task: AngularFireUploadTask;
  percentage: Observable<number>;
  snapshot: Observable<any>;
  downloadURL: string;
  files: File[] = [];
  actionValues: any = [];
  workFlowTemplateID: any;
  dynamicForm:any;
  // formDataEdit = new AddGroupIdName();
  constructor(private storage: AngularFireStorage, private db: AngularFirestore, @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddNewRequestComponent>, private toastr: ToastrService,
    private router: Router, private loadStaffAcountService: LoadStaffAcountService) { }


  addURLtoList(event) {
    this.listURL.push(event.toString());
  }
  toggleHover(event: boolean) {
    this.isHovering = event;
  }
  sendReqNextStep(nextStepID){
    debugger;

    this.dynamicForm.forEach(element => {
      if(element.textOnly.name.toString() != "" ){
        this.actionValues.push({ "key": element.textOnly.name.toString(), "value": ""});
      }
      if(element.shortText.name.toString() != "" ){
        this.actionValues.push({ "key": element.shortText.name.toString(), "value": element.shortText.value.toString()});
      }
      if(element.longText.name.toString() != "" ){
        this.actionValues.push({ "key": element.longText.name.toString(), "value": element.longText.value.toString()});
      }
      if(element.comboBox.name.toString() != "" ){
        this.actionValues.push({ "key": element.comboBox.name.toString(), "value": element.comboBox.value.toString()});
      }
      if(element.inputCheckbox.name.toString() != "" ){
        this.actionValues.push({ "key": element.inputCheckbox.name.toString(), "value": element.inputCheckbox.value.toString()});
      }
      
    });
    




    //this.actionValues.push({ "key": this.formKey, "value": this.formValue})
    var mdSendReq = new SendRequest("", this.actionValues, this.listURL, this.workFlowTemplateID.toString(), nextStepID.toString());
    console.log(JSON.stringify(mdSendReq));
    
    this.loadStaffAcountService.sendReq(mdSendReq).toPromise().then(data =>{
      this.toastr.success('Success! ' , '' );
      console.log(this.downloadURL);
      console.log("aaaa");
      console.log(this.listURL);
      this.dialogRef.close();
    }, (err) => {
        this.toastr.error("Error:" + err.error, "Something wrong!" );
      });
    
  }
  
  onDrop(files: FileList) {
    for (let i = 0; i < files.length; i++) {
      this.files.push(files.item(i));
    }
    // this.files.forEach(file => {
      


    //   // The storage path
    //   const path = `test/${Date.now()}_${file.name}`;

    //   // Reference to storage bucket
    //   const ref = this.storage.ref(path);

    //   // The main task
    //   this.task = this.storage.upload(path, file);

    //   // Progress monitoring
    //   this.percentage = this.task.percentageChanges();

    //   this.snapshot = this.task.snapshotChanges().pipe(
    //     tap(console.log),
    //     // The file's download URL
    //     finalize(async () => {
    //       this.downloadURL = await ref.getDownloadURL().toPromise();
          
    //       this.db.collection('files').add({ downloadURL: this.downloadURL, path });      
    //       var a = this.downloadURL;
    //       this.listURL.push(a);
    //     }),
    //   );

    //});

  }
  isActive(snapshot) {
    return snapshot.state === 'running' && snapshot.bytesTransferred < snapshot.totalBytes;
  }



  ngOnInit() {

    this.workFlowTemplateID = this.data;
    this.loadStaffAcountService.getRequestForm(this.workFlowTemplateID).toPromise().then(res => {
      
      this.saveData = res;
      console.log(this.saveData);
      this.buttons = this.saveData.connections;
      this.formKey = this.saveData.actionType.name;
      this.dynamicForm = JSON.parse(this.saveData.actionType.data);
      console.log(this.formKey);
    },err =>{
      this.toastr.error(err.error);
    })

  }
  onSubmit() {
    //   this.loadStaffAcountService.addPermission(this.formData).toPromise().then(    
    //     resp => {    
    //       console.log(resp.toString())   ;  

    //     });    
    // this.toastr.success('Success! ' , '' );
    // this.dialogRef.close();
  }

}
