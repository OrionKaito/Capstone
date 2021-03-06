import { Injectable } from '@angular/core';  
import {HttpClient} from '@angular/common/http';  
import {HttpHeaders} from '@angular/common/http';  
import { from, Observable } from 'rxjs';  
import { Text } from '@angular/compiler/src/i18n/i18n_ast';

@Injectable({  
  providedIn: 'root'  
})  
export class LoginService {  
  Url :string;  
  token : string;  
  header : any;
  
  constructor(private http : HttpClient) {   
  
    this.Url = 'https://api.workflow.demo.saigontechnology.vn';  
  
    const headerSettings: {[name: string]: string | string[]; } = {};  
    this.header =  new HttpHeaders(headerSettings);  
  }  
  Login(model : any){  
    debugger;  
     var a =this.Url + "/api/Token/Admin";  
   return  this.http.post(a,model,{responseType: 'text'});  
  }  
  Register(model : any){
     var b =this.Url + "/api/Accounts";   
   return this.http.post(b,model,{responseType: 'text'}); 
  }
  editAccount(model : any){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    //var b =this.Url + "/api/Accounts/PutAccountByAdmin?ID=" + model.id;   
    var b =this.Url + "/api/Accounts/PutAccountByAdmin"

  return this.http.put(b,model,{headers : tokenHeader, responseType: "text" }); 
 }

  getUserProfile(){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    return this.http.get(this.Url + "/api/Accounts/GetProfile", {headers : tokenHeader });
  }
  updateUserProfile(a,b){
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    var body= {"fullName":a,"dateOfBirth":b};
    return this.http.put(this.Url + "/api/Accounts/UpdateProfile", body, {headers : tokenHeader, responseType: "text" });
  }

  BanOrUnbanAcc(id: string){
    const headers = new HttpHeaders({ 'Content-Type': 'application/json'});
    return this.http.put(this.Url + "/api/Accounts/ToggleBanAccount?ID=" +id, {headers: headers });
  }
  changePass(cur: any, newP: any ){
    debugger;
    var token = "Bearer " + localStorage.getItem("token");
    var tokenHeader = new HttpHeaders({'Authorization': token});
    let thisURL= this.Url + "/api/Accounts/ChangePassword?oldPassword="+ cur +"&newPassword="+ newP;
    return this.http.put( thisURL, {"oldPassword": cur, "newPassword": newP} , {headers : tokenHeader , responseType: 'text' });
  }
 
}  