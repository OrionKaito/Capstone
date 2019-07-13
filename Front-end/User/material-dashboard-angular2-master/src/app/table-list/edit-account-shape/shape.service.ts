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
      id= "c2052087-a290-4b70-9742-08d70789ab22";
      return this.http.get(this.urlApi + '/api/ActionTypes/GetByID?ID=' + id);

    }

}
