import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LoadStaffAcountService {
  Url :string;  
  token : string; 
  constructor(private http : HttpClient) {
    this.Url = 'https://localhost:44359/api/Accounts';
    this.token = localStorage.getItem('token');
   }
   loadStaffData(){
    debugger;
     return this.http.get(this.Url);
   }
    
}
