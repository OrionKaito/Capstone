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
import { AddRoleComponent } from './add-role/add-role.component';
import { AddGroupComponent } from './add-group/add-group.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  imports: [
    AngularMultiSelectModule,
    NgMultiSelectDropDownModule.forRoot(),
    ToastrModule.forRoot(),
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
    AddRoleComponent,
    AddGroupComponent,
  ],
  entryComponents:[AddAccountComponent, AddRoleComponent,AddGroupComponent],

  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
