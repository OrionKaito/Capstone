import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LoadStaffAcountService {
  Url :string;  
  token : string; 
  constructor(private http : HttpClient) {
    this.Url = 'https://localhost:44359';
    this.token = localStorage.getItem('token');
   }
   loadStaffData(){
    debugger;
     return this.http.get(this.Url+"/api/Accounts");
   }
   loadRoleData(){
    debugger;
     return this.http.get(this.Url+"/api/Roles");
   }
   deleteRole(id: string){  
      return this.http.delete(this.Url + "/api/Roles?ID=" +id);
   }
   addRole(model : any){
      return this.http.post(this.Url + "/api/Roles",model,{responseType: 'text'}); 
   }
   loadGroupData(){
    debugger;
    return this.http.get(this.Url+"/api/Groups");
   }
   deleteGroup(id: string){  
    return this.http.delete(this.Url + "/api/Groups?ID=" +id);
 }
    
}
