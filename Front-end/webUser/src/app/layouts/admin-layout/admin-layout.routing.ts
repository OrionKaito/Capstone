import { Routes } from '@angular/router';

import { DashboardComponent } from '../../dashboard/dashboard.component';
import { TableListComponent } from '../../table-list/table-list.component';
import { UserProfileComponent } from 'app/user-profile/user-profile.component';
import { ManagePermissionComponent } from 'app/manage-permission/manage-permission.component';

import { WorkflowOfUserComponent } from 'app/workflow-of-user/workflow-of-user.component';
import { HandleRequestComponent } from 'app/handle-request/handle-request.component';
import { ManagePerGrComponent } from 'app/manage-per-gr/manage-per-gr.component';
import { ManageYourRequestComponent } from 'app/manage-your-request/manage-your-request.component';
import { ManageErrRequestComponent } from 'app/manage-err-request/manage-err-request.component';


export const AdminLayoutRoutes: Routes = [
    { path: 'dashboard',      component: DashboardComponent },
    { path: 'user-profile',   component: UserProfileComponent },
    { path: 'manage-workflow',     component: TableListComponent },
    { path: 'manage-permission',     component: ManagePermissionComponent },
    { path: 'err-request',     component: ManageErrRequestComponent},
    { path: 'your-request',     component: ManageYourRequestComponent },
    { path: 'manage-per-gr',     component: ManagePerGrComponent },
    { path: 'create-request',     component: WorkflowOfUserComponent },
    { path: 'handle-request',     component: HandleRequestComponent }
];


