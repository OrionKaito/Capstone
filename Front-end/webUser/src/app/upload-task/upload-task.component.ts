import { Component, OnInit, Input, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { AngularFireStorage, AngularFireUploadTask } from '@angular/fire/storage';
import { AngularFirestore } from '@angular/fire/firestore';
import { Observable } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-upload-task',
  templateUrl: './upload-task.component.html',
  styleUrls: ['./upload-task.component.scss']
})
export class UploadTaskComponent implements OnInit {

  @Input() file: File;
  @Output('getURL') returnURL = new EventEmitter<string>();
  task: AngularFireUploadTask;
  percentage: Observable<number>;
  snapshot: Observable<any>;
  downloadURL: string;
  linkUrl: any;

  constructor(private toastr: ToastrService, private storage: AngularFireStorage, private db: AngularFirestore, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() {
    this.startUpload();
  }

  startUpload() {
    const fd=  new FormData();
    const path = `${Date.now()}_${this.file.name}`;
    fd.append(path, this.file);
  
    this.loadStaffAcountService.upLoadFileToServe(fd).toPromise().then(
      res=>{
        this.linkUrl=res;
        this.downloadURL = this.linkUrl;
        this.returnURL.emit(this.downloadURL);
      },err =>{
        this.toastr.error(err.error);
      }
    )

    // // The storage path
    // const path = `test/${Date.now()}_${this.file.name}`;

    // // Reference to storage bucket
    // const ref = this.storage.ref(path);

    // // The main task
    // this.task = this.storage.upload(path, this.file);

    // // Progress monitoring
    // this.percentage = this.task.percentageChanges();

    // this.snapshot = this.task.snapshotChanges().pipe(
    //   tap(console.log),
    //   // The file's download URL
    //   finalize(async () => {
    //     this.downloadURL = await ref.getDownloadURL().toPromise();
        
    //     this.db.collection('files').add({ downloadURL: this.downloadURL, path });

    //     this.returnURL.emit(this.downloadURL)

    //   }),
    // );

  }

  isActive(snapshot) {
    return snapshot.state === 'running' && snapshot.bytesTransferred < snapshot.totalBytes;
  }

}
