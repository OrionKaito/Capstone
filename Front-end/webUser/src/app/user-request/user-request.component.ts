import { Component, OnInit } from '@angular/core';
import "firebase/auth";
import "firebase/firestore";
import { AngularFirestore } from '@angular/fire/firestore';
import { SendRequest } from 'app/useClass/send-request';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-user-request',
  templateUrl: './user-request.component.html',
  styleUrls: ['./user-request.component.scss']
})

export class UserRequestComponent implements OnInit {
  listURL: any=[];
  isHovering: boolean;
  value: any

  files: File[] = [];
  addURLtoList(event){
    this.listURL.push(event);
  }
  toggleHover(event: boolean) {
    this.isHovering = event;
  }

  onDrop(files: FileList) {
    for (let i = 0; i < files.length; i++) {
      this.files.push(files.item(i));
    }
  }
  constructor(private toastr: ToastrService, private loadStaffAcountService: LoadStaffAcountService) { }
 
  ngOnInit() {
  }

  sendReq(){
    // var mdSendRequest = new SendRequest("Đây là đơn xin nghỉ phép", [{"lydo": this.value}],this.listURL);
    // this.loadStaffAcountService.sendReq(mdSendRequest).toPromise().then(data =>{
    //   this.toastr.success('Success! ' , '' );
    // }
    // )
  }
  


}
