import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminLayoutRoutes } from './admin-layout.routing';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import { UserProfileComponent } from '../../user-profile/user-profile.component';
import { TableListComponent } from '../../table-list/table-list.component'

import {
  MatButtonModule,
  MatInputModule,
  MatRippleModule,
  MatFormFieldModule,
  MatTooltipModule,
  MatSelectModule,
  MatTableModule,
  MatIconModule,
  MatPaginatorModule,
  MatSortModule,
  MatDialogModule
} from '@angular/material';

import { CdkTableModule } from '@angular/cdk/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AddAccountComponent } from 'app/add-account/add-account.component';
import { BrowserModule } from '@angular/platform-browser';
import { ToastrModule } from 'ngx-toastr';
import { ManagePermissionComponent } from 'app/manage-permission/manage-permission.component';
import { AngularFireModule } from '@angular/fire';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { AngularFireStorageModule } from '@angular/fire/storage';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { UploadTaskComponent } from 'app/upload-task/upload-task.component';
import { WorkflowOfUserComponent } from 'app/workflow-of-user/workflow-of-user.component';
import { HandleRequestComponent } from 'app/handle-request/handle-request.component';
import { ManagePerGrComponent } from 'app/manage-per-gr/manage-per-gr.component';
import { ManageYourRequestComponent } from 'app/manage-your-request/manage-your-request.component';

// const config = {
//   apiKey: "AIzaSyCBhMb1lO_1ioNypRrHS4I1q9sYxJ2thcs",
//     authDomain: "capstonedinamicworkflow.firebaseapp.com",
//     databaseURL: "https://capstonedinamicworkflow.firebaseio.com",
//     projectId: "capstonedinamicworkflow",
//     storageBucket: "capstonedinamicworkflow.appspot.com",
//     messagingSenderId: "1002298812738",
//     appId: "1:1002298812738:web:3979cfb21aa67003"
// };

@NgModule({
  imports: [
    NgMultiSelectDropDownModule.forRoot(),
 //   AngularFireModule.initializeApp(config),
    AngularFirestoreModule, // firestore
    AngularFireAuthModule, // auth
    AngularFireStorageModule, // storage
    ToastrModule.forRoot(),
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatDialogModule,
    MatRippleModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule,
    CdkTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatIconModule,
    MatTooltipModule,
  ], exports: [
    MatPaginatorModule,
    MatSortModule
  ],
  declarations: [
    DashboardComponent,
    UserProfileComponent,
    TableListComponent,
    ManagePermissionComponent,
    WorkflowOfUserComponent,
    HandleRequestComponent,
    ManagePerGrComponent,
    ManageYourRequestComponent
  ]
})

export class AdminLayoutModule {}
