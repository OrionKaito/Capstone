import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import {HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app.routing';
import { ComponentsModule } from './components/components.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import {
  AgmCoreModule
} from '@agm/core';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTabsModule, MatIconModule, MatPaginatorModule, MatSortModule, MatDialogModule } from '@angular/material';
import { CdkTable, CdkTableModule } from '@angular/cdk/table';
import { AddAccountComponent } from './add-account/add-account.component';
import { ToastrModule } from 'ngx-toastr';
import { ManagePermissionComponent } from './manage-permission/manage-permission.component';
import { AddPermissionComponent } from './add-permission/add-permission.component';
import * as firebase from "firebase/app";
import { AngularFireModule } from '@angular/fire';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { AngularFireStorageModule } from '@angular/fire/storage';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { DropzoneDirective } from './dropzone.directive';
import { UploadTaskComponent } from './upload-task/upload-task.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { AddNewRequestComponent } from './add-new-request/add-new-request.component';
import { HandleRequestComponent } from './handle-request/handle-request.component';
import { AddHandleRequestComponent } from './add-handle-request/add-handle-request.component';
import { SetGroupPermissionComponent } from './set-group-permission/set-group-permission.component';


const config = {
  apiKey: "AIzaSyCBhMb1lO_1ioNypRrHS4I1q9sYxJ2thcs",
    authDomain: "capstonedinamicworkflow.firebaseapp.com",
    databaseURL: "https://capstonedinamicworkflow.firebaseio.com",
    projectId: "capstonedinamicworkflow",
    storageBucket: "capstonedinamicworkflow.appspot.com",
    messagingSenderId: "1002298812738",
    appId: "1:1002298812738:web:3979cfb21aa67003"
};
@NgModule({
  imports: [
    NgMultiSelectDropDownModule.forRoot(),
    ToastrModule.forRoot(),
    AngularFireModule.initializeApp(config),
    AngularFirestoreModule, // firestore
    AngularFireAuthModule, // auth
    AngularFireStorageModule, // storage
    BrowserAnimationsModule,
    CommonModule,
    MatIconModule,
    CdkTableModule,
    MatPaginatorModule,
    MatSortModule,
    FormsModule,
    HttpModule,
    ComponentsModule,
    HttpClientModule,
    MatDialogModule,
    RouterModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    AgmCoreModule.forRoot({
      apiKey: 'YOUR_GOOGLE_MAPS_API_KEY'
    })


  ],
  exports: [
    MatPaginatorModule,
    MatSortModule
  ],
  declarations: [
    AppComponent,
    AdminLayoutComponent,
    LoginComponent,
    AddAccountComponent,
    AddPermissionComponent,
    AddNewRequestComponent,
    DropzoneDirective,
    UploadTaskComponent,
    AddHandleRequestComponent,
    SetGroupPermissionComponent,
  ],
  entryComponents:[AddAccountComponent, AddPermissionComponent, AddNewRequestComponent, AddHandleRequestComponent ],

  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
