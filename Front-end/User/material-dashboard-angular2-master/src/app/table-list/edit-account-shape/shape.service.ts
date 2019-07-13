import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class ShapeService {
  public urlApi = 'https://localhost:44359';
  // public token = 'Bearer ' + localStorage.getItem('token');
  // public tokenHeader: any;
  constructor(
    private http: HttpClient
  ) {
  // this.tokenHeader = new HttpHeaders({'Authorization': this.token});
   }

    public getDropdownList() {
      return this.http.get(this.urlApi  + "/api/Permissions");
    }

    public postJsonFile(json) {
      // return this.http.post(this.urlApi + 'post-data-json', json, {headers : this.tokenHeader });
      var send = {
        "name": json,
        "data": json
      }
      return this.http.post(this.urlApi + '/api/ActionTypes', send);
    }

    public getJsonByUserId(id) {
      id= "7397e241-baac-4d6b-d77d-08d707a80d16";
      return this.http.get(this.urlApi + '/api/ActionTypes/GetByID?ID=' + id);
    }

}
