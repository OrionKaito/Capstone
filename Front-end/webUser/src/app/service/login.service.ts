import { Injectable } from '@angular/core';  
import {HttpClient} from '@angular/common/http';  
import {HttpHeaders} from '@angular/common/http';  
import { from, Observable } from 'rxjs';  
import { Text } from '@angular/compiler/src/i18n/i18n_ast';
import { GlobalVar } from 'app/useClass/global-var';

@Injectable({  
  providedIn: 'root'  
})  
export class LoginService {  
  Url :string;  
  token : string;  
  header : any;
  
  constructor(private http : HttpClient) {   
  
    this.Url = GlobalVar.url;  
  
    const headerSettings: {[name: string]: string | string[]; } = {};  
    this.header =  new HttpHeaders(headerSettings);  
  }  
  Loginv2(model : any){  
    debugger;  
    var a =this.Url + "api/Token/TestLogin";  
    return  this.http.post(a,model, { observe: 'response' });  
  }  
  Login(model : any){  
    debugger;  

     var a =this.Url + "api/Token/NewLogin";
    //  var a =this.Url + "api/Token/User";
   return  this.http.post(a,model, { observe: 'response' });  
  }  
  Register(model : any){
     var b =this.Url + "api/Accounts";   

   return this.http.post(b,model,{responseType: 'text'}); 
  }
  addNewWF(model : any){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
     var b =this.Url + "api/WorkflowsTemplates";   
   return this.http.post(b,model,{headers : tokenHeader }); 
  }
  editWF(model : any){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
     var b =this.Url + "api/WorkflowsTemplates/SaveDraft"; 
   return this.http.put(b,model,{headers : tokenHeader, responseType: 'text' }); 
  }

  getUserProfile(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "api/Accounts/GetProfile", {headers : tokenHeader });
  }

  updateUserProfile(a,b){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    var body= {"fullName":a,"dateOfBirth":b, "imagePath": ""};
    return this.http.put(this.Url + "api/Accounts/UpdateProfile", body, {headers : tokenHeader , responseType: 'text' });
  }
  changePass(cur: any, newP: any ){
    debugger;
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    let thisURL= this.Url + "api/Accounts/ChangePassword?oldPassword="+ cur +"&newPassword="+ newP;
    return this.http.put( thisURL, {"oldPassword": cur, "newPassword": newP} , {headers : tokenHeader , responseType: 'text' });
  }

  BanOrUnbanAcc(id: string){
    const headers = new HttpHeaders({ 'Content-Type': 'application/json'}); 
    return this.http.put(this.Url + "api/Accounts/ToggleBanAccount?ID=" +id, {"ID": id},{ headers: headers });
  }
  BanOrUnbanPer(id){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
   
    return this.http.delete( this.Url+"api/Permissions?ID="+id , {headers : tokenHeader , responseType: 'text' });
  }
  enabledDisableWF(id: string){
    const headers = new HttpHeaders({ 'Content-Type': 'application/json'}); 
    return this.http.put(this.Url + "api/WorkflowsTemplates/ToggleEnable?ID=" +id, {"ID": id},{ headers: headers });
  }
  resetPassword(email: string){
    return this.http.post(this.Url + 'api/Accounts/ForgotPassword?email=' + email, {'email': email} ,{ responseType: 'text'});
  }
  sendCodeConfig(code: string, email: string, password: string ) {

    return this.http.put(this.Url + 'api/Accounts/ConfirmForgotPassword?code=' + code + '&email=' + email + '&password=' + password,
        {'code': code, 'email': email, 'password': password}, { responseType: 'text'});
  }
}  
