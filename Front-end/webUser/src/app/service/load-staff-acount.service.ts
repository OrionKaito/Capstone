import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LoadStaffAcountService {
  Url: string;
  token: string;
  constructor(private http: HttpClient) {
    this.Url = 'https://localhost:44359';
    this.token = localStorage.getItem('token');
  }
  checkRole(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "", {headers : tokenHeader });
  }
  loadStaffData() {
    return this.http.get(this.Url + "/api/Accounts");
  }
  loadWorkFlow(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "/api/WorkflowsTemplates", {headers : tokenHeader });
  }
  loadWFByID(id: string){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "/api/WorkflowsTemplates", {headers : tokenHeader });
  }
  loadPermissionOfRoleData() {
    return this.http.get(this.Url + "/api/PermissionOfRoles");
  }
  deletePerOfRole(id: string) {
    return this.http.delete(this.Url + "/api/PermissionOfRoles?ID=" + id);
  }
  loadRoleData() {
    debugger;
    return this.http.get(this.Url + "/api/Roles");
  }
  deleteRole(id: string) {
    return this.http.delete(this.Url + "/api/Roles?ID=" + id);
  }
  addRole(model: any) {
    return this.http.post(this.Url + "/api/Roles", model, { responseType: 'text' });
  }
  editRole(model: any) {
    return this.http.put(this.Url + "/api/Roles", model);
  }
  loadRoleByID(id) {
    return this.http.get(this.Url + "/api/Roles/GetByID?ID=" + id);
  }
  deleteGroup(id: string) {
    return this.http.delete(this.Url + "/api/Groups?ID=" + id);
  }
  addGroup(model: any) {
    return this.http.post(this.Url + "/api/Groups", model, { responseType: 'text' });
  }
  editGroup(model: any) {
    return this.http.put(this.Url + "/api/Groups", model);
  }
  loadGroupData() {
    return this.http.get(this.Url + "/api/Groups");
  }
  loadWorkflowForUserData(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "/api/WorkflowsTemplates/GetWorkflowToUse", {headers : tokenHeader });
  }

  loadHandlingRequest(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "/api/UserNotifications/GetNotificationByUserId?notificationType=2", {headers : tokenHeader });
  }
  loadPermissionGroupData() {
    return this.http.get(this.Url + "/api/PermissionOfGroups/GetPermissionByGroup");
  }

  loadPermissionData() {
    return this.http.get(this.Url + "/api/Permissions");
  }
  deletePermission(id: string) {
    return this.http.delete(this.Url + "/api/Permissions?ID=" + id);
  }
  addPermission(model: any) {
    return this.http.post(this.Url + "/api/Permissions", model, { responseType: 'text' });
  }
  editPermission(model: any) {
    return this.http.put(this.Url + "/api/Permissions", model);
  }
  loadPermissionByID(id) {
    return this.http.get(this.Url + "/api/Permissions/GetByID?ID=" + id);

  }
  addPermissionGr(model: any) {
    return this.http.post(this.Url + "/api/PermissionOfGroups", model);
  }

  getRequestForm(id){
    return this.http.get(this.Url + "/api/Requests/GetRequestForm?workFlowTemplateID=" + id);
  }
  getHandleForm(id){
    return this.http.get(this.Url + "/api/Requests/GetRequestHandleForm?requestActionID=" + id);
  }


  getRolebyID(id) {
    return this.http.get(this.Url + "/api/Roles/GetByUserID?ID=" + id, { responseType: 'json' });
  }
  getGroupbyID(id) {
    return this.http.get(this.Url + "/api/Groups/GetByUserID?ID=" + id, { responseType: 'json' });
  }
  loadUserByID(id) {
    return this.http.get(this.Url + "/api/Accounts/GetAccountByUserID?ID=" + id);
  }


  loadGroupByID(id) {
    return this.http.get(this.Url + " /api/Groups/GetByID?ID=" + id);

  }
  sendReq(req){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.post(this.Url + "/api/Requests", req, {headers : tokenHeader });
  }
  sendReqHandle(req){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.post(this.Url + "/api/Requests/ApproveRequest", req, {headers : tokenHeader });
  }
  upLoadFileToServe(req){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.post(this.Url + "/api/RequestFiles", req, {headers : tokenHeader });
  }

}
