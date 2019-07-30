import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AngularFireDatabase } from 'angularfire2/database';
import { AngularFireAuth } from 'angularfire2/auth';
import * as firebase from 'firebase/app';
import 'rxjs/add/operator/take';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { GlobalVar } from 'app/useClass/global-var';


@Injectable({
  providedIn: 'root'
})
export class LoadStaffAcountService {
  messaging = firebase.messaging();
  currentMessage = new BehaviorSubject(null); 
  Url: string;
  token: string;

  constructor(private http: HttpClient, private db: AngularFireDatabase, private afAuth: AngularFireAuth) {


    this.Url = GlobalVar.url;
    this.token = localStorage.getItem('token');
  }

  private updateToken(token){
    this.afAuth.authState.take(1).toPromise().then(user =>{
      if(!user) return;
      const data ={[user.uid]: token};
      this.db.object('fcmTokens/').update(data);
    })
  }
  getPermission(){
    this.messaging.requestPermission()
    .then(()=>{
      console.log('Notification permission granted.');
      return this.messaging.getToken();
    }).then(token=>{
      console.log(token);
      this.updateToken(token);
      localStorage.setItem("tokenNoti", token);

    }).catch((err)=>{
      console.log('Unable to get permission to notify.', err);
    });
  }
  
  receiveMessage(){
    this.messaging.onMessage((payload)=>{
       console.log("Message received. ", payload);
        this.currentMessage.next(payload);
    });
  }
  // sendDiviceIdToDtb(deviceID){
  //   var token = "Bearer " + localStorage.getItem("token");
  //   var tokenHeader = new HttpHeaders({'Authorization': token});
  //   return this.http.get(this.Url + "/api/UserNotifications/GetNumberOfNotification=3", deviceID, {headers : tokenHeader });
  // }






  getNotiUser(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/UserNotifications/GetNotificationByUser", {headers : tokenHeader });
  }
  getNumNotiUser(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/UserNotifications/GetNumberNotification", {headers : tokenHeader });
  }
  checkRole(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/Token/GetRole", {headers : tokenHeader });
  }
  loadStaffData() {
    return this.http.get(this.Url + "api/Accounts");
  }
  loadWorkFlow(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/WorkflowsTemplates/GetWorkflowToEdit", {headers : tokenHeader });
  }
  loadWFByID(id: string){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/WorkflowsTemplates/GetByID?ID="+ id, {headers : tokenHeader });
  }
  loadPermissionOfRoleData() {
    return this.http.get(this.Url + "api/PermissionOfRoles");
  }
  deletePerOfRole(id: string) {
    return this.http.delete(this.Url + "api/PermissionOfRoles?ID=" + id);
  }
  loadRoleData() {
    debugger;
    return this.http.get(this.Url + "api/Roles");
  }
  deleteRole(id: string) {
    return this.http.delete(this.Url + "api/Roles?ID=" + id);
  }
  addRole(model: any) {
    return this.http.post(this.Url + "api/Roles", model, { responseType: 'text' });
  }
  editRole(model: any) {
    return this.http.put(this.Url + "api/Roles", model);
  }
  loadRoleByID(id) {
    return this.http.get(this.Url + "api/Roles/GetByID?ID=" + id);
  }
  deleteGroup(id: string) {
    return this.http.delete(this.Url + "api/Groups?ID=" + id);
  }
  addGroup(model: any) {
    return this.http.post(this.Url + "api/Groups", model, { responseType: 'text' });
  }
  editGroup(model: any) {
    return this.http.put(this.Url + "api/Groups", model);
  }
  loadGroupData() {
    return this.http.get(this.Url + "api/Groups");
  }
  loadWorkflowForUserData(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/WorkflowsTemplates/GetWorkflowToUse", {headers : tokenHeader });
  }

  loadHandlingRequest(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/Requests/GetRequestsToHandleByPermission", {headers : tokenHeader });
  }
  loadPermissionGroupData() {
    return this.http.get(this.Url + "api/PermissionOfGroups/GetPermissionByGroup");
  }

  loadPermissionData() {
    return this.http.get(this.Url + "api/Permissions");
  }
  deletePermission(id: string) {
    return this.http.delete(this.Url + "api/Permissions?ID=" + id);
  }
  addPermission(model: any) {
    return this.http.post(this.Url + "api/Permissions", model, { responseType: 'text' });
  }
  editPermission(model: any) {
    return this.http.put(this.Url + "api/Permissions", model);
  }
  loadPermissionByID(id) {
    return this.http.get(this.Url + "api/Permissions/GetByID?ID=" + id);

  }
  addPermissionGr(model: any) {
    return this.http.post(this.Url + "api/PermissionOfGroups", model);
  }
  loadYourRequest(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/Requests/GetMyRequests", {headers : tokenHeader });

  }
  getRequestForm(id){
    return this.http.get(this.Url + "api/Requests/GetRequestForm?workFlowTemplateID=" + id);
  }
  getHandleForm(id){
    return this.http.get(this.Url + "api/Requests/GetRequestHandleForm?requestActionID=" + id);
  }


  getRolebyID(id) {
    return this.http.get(this.Url + "api/Roles/GetByUserID?ID=" + id, { responseType: 'json' });
  }
  getGroupbyID(id) {
    return this.http.get(this.Url + "api/Groups/GetByUserID?ID=" + id, { responseType: 'json' });
  }
  loadUserByID(id) {
    return this.http.get(this.Url + "api/Accounts/GetAccountByUserID?ID=" + id);
  }


  loadGroupByID(id) {
    return this.http.get(this.Url + "api/Groups/GetByID?ID=" + id);

  }
  sendReq(req){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.post(this.Url + "api/Requests", req, {headers : tokenHeader });
  }
  sendReqHandle(req){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.post(this.Url + "api/Requests/ApproveRequest", req, {headers : tokenHeader });
  }
  upLoadFileToServe(req){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.post(this.Url + "api/RequestFiles", req, {headers : tokenHeader });
  }
  createAction(action){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.post(this.Url + "api/ActionTypes", action, {headers : tokenHeader });
  }
  loadPermissionByGr(id){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/PermissionOfGroups/GetByGroup?ID=" + id, {headers : tokenHeader });
  }
  putPerGr(model){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.put(this.Url + "api/PermissionOfGroups/CreateOrEditPermissionByGroup",model, {headers : tokenHeader, responseType: "text" });

  }
  getYourRequest(id){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/Requests/GetRequestResult?requestActionID=" +id, {headers : tokenHeader});
  }

}
