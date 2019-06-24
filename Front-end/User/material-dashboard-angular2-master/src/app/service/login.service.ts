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
  
    this.Url = 'https://localhost:44359';  
  
    const headerSettings: {[name: string]: string | string[]; } = {};  
    this.header =  new HttpHeaders(headerSettings);  
  }  
  Login(model : any){  
    debugger;  
     var a =this.Url + "/api/Token/User";  
   return  this.http.post(a,model,{responseType: 'text'});  
  }  
  Register(model : any){
   
    
     var b =this.Url + "/api/Accounts";   
   return this.http.post(b,model,{responseType: 'text'}); 
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
    return this.http.put(this.Url + "/api/Accounts", body, {headers : tokenHeader });
  }

  BanOrUnbanAcc(id: string){
    const headers = new HttpHeaders({ 'Content-Type': 'application/json'}); 
    return this.http.put(this.Url + "/api/Accounts/ToggleBanAccount?ID=" +id, {"ID": id},{ headers: headers });
  }
 
}  