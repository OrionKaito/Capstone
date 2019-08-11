import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
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
import {
    MatTableModule,
    MatTabsModule,
    MatCheckboxModule,
    MatIconModule,
    MatPaginatorModule,
    MatSortModule,
    MatDialogModule,
    MatSelectModule,
    MatFormFieldModule,
    MatChipsModule,
    MatInputModule,
    MatCardModule, MatMenuModule
} from '@angular/material';
import { CdkTable, CdkTableModule } from '@angular/cdk/table';
import { AddAccountComponent } from './add-account/add-account.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ToastrModule } from 'ngx-toastr';
import { ManagePermissionComponent } from './manage-permission/manage-permission.component';
import { AddPermissionComponent } from './add-permission/add-permission.component';
import '@firebase/messaging';
import * as firebase from "firebase/app";
import { AngularFireModule } from '@angular/fire';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { AddNewRequestComponent } from './add-new-request/add-new-request.component';
import { HandleRequestComponent } from './handle-request/handle-request.component';
import { AddHandleRequestComponent } from './add-handle-request/add-handle-request.component';
import { SetGroupPermissionComponent } from './set-group-permission/set-group-permission.component';
import { AngularFireStorageModule } from '@angular/fire/storage';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { DropzoneDirective } from './dropzone.directive';
import { UploadTaskComponent } from './upload-task/upload-task.component';
import { EditAccountShapeComponent } from './table-list/edit-account-shape/edit-account-shape.component';
import { MenuEditAccountShapeComponent } from './table-list/edit-account-shape/menu-edit-account-shape/menu-edit-account-shape.component';
import { MDBBootstrapModule, ButtonsModule, WavesModule,
  InputsModule, CollapseModule, ModalModule, IconsModule } from 'angular-bootstrap-md';
import { AddNewDynamicFormComponent } from './add-new-dynamic-form/add-new-dynamic-form.component';
import { ManagePerGrComponent } from './manage-per-gr/manage-per-gr.component';
import { AddManagePerGrComponent } from './add-manage-per-gr/add-manage-per-gr.component';
import { AngularFireDatabase, AngularFireDatabaseModule } from 'angularfire2/database';
import { ManageYourRequestComponent } from './manage-your-request/manage-your-request.component';
import { AddYourRequestComponent } from './add-your-request/add-your-request.component';

const config = {
  apiKey: "AIzaSyA870TvgTsit4CcKLI0hnn3v89-yR6mwLE",
  authDomain: "capstoneproject-248a5.firebaseapp.com",
  databaseURL: "https://capstoneproject-248a5.firebaseio.com",
  projectId: "capstoneproject-248a5",
  storageBucket: "capstoneproject-248a5.appspot.com",
  messagingSenderId: "499817926017",
  appId: "1:499817926017:web:027e25c5294ad5dc"

};
@NgModule({
    imports: [

        NgMultiSelectDropDownModule.forRoot(),
        ToastrModule.forRoot(),
        AngularFireModule.initializeApp(config),
        AngularFirestoreModule, // firestore
        AngularFireAuthModule, // auth
        AngularFireStorageModule, // storage
        AngularFireDatabaseModule,
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
        MatCheckboxModule,
        RouterModule,
        AppRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        MatTableModule,
        AgmCoreModule.forRoot({
            apiKey: 'YOUR_GOOGLE_MAPS_API_KEY'
        }),
        ButtonsModule, WavesModule, CollapseModule, InputsModule, IconsModule,
        MatSelectModule, MatFormFieldModule, MatChipsModule, MatInputModule,
        MDBBootstrapModule.forRoot(), MatCardModule, MatMenuModule


    ],
  exports: [
    MatPaginatorModule,
    MatSortModule
  ],
  declarations: [
    AppComponent,
    AdminLayoutComponent,
    LoginComponent,
    AddNewRequestComponent,
    AddAccountComponent,
    AddPermissionComponent,
    AddManagePerGrComponent,
    DropzoneDirective,
    EditAccountShapeComponent,
    MenuEditAccountShapeComponent,
    UploadTaskComponent,
    AddHandleRequestComponent,
    SetGroupPermissionComponent,
    AddNewDynamicFormComponent,
    AddManagePerGrComponent,
    AddYourRequestComponent,


  ],
  
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  entryComponents:[
    AddAccountComponent, 
    AddManagePerGrComponent,
     AddNewDynamicFormComponent,
     AddManagePerGrComponent, 
     AddPermissionComponent, 
     AddNewRequestComponent, 
     AddHandleRequestComponent,
    AddYourRequestComponent ],

  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
