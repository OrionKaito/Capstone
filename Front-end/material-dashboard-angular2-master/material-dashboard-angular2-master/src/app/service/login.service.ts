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
     var a =this.Url + "/api/Token/Admin";  
   return  this.http.post(a,model,{responseType: 'text'});  
  }  
  Register(model : any){
    debugger;  
     var b =this.Url + "/api/Accounts";   
   return  this.http.post(b,model,{responseType: 'text'}); 
  }
 
}  