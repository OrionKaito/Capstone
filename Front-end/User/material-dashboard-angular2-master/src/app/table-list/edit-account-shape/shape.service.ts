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
    public getActiontype() {
      return this.http.get(this.urlApi  + "/api/ActionTypes");
    }
    public getConnectionType(){
      return this.http.get(this.urlApi  + "/api/ConnectionTypes");
    }


    public postJsonFile(json) { 
      // return this.http.post(this.urlApi + 'post-data-json', json, {headers : this.tokenHeader });
      var token = "Bearer " + localStorage.getItem("token");
      var tokenHeader = new HttpHeaders({'Authorization': token}); 
      return this.http.put(this.urlApi + '/api/WorkflowsTemplates/drafJson', json , {headers : tokenHeader });
    }
    public saveAndACtiveJsonFile(json) { 
      // return this.http.post(this.urlApi + 'post-data-json', json, {headers : this.tokenHeader });
      var token = "Bearer " + localStorage.getItem("token");
      var tokenHeader = new HttpHeaders({'Authorization': token}); 
      return this.http.put(this.urlApi + '/api/WorkflowsTemplates/saveJson', json , {headers : tokenHeader });
    }

    public getJsonByUserId(id) {   
      return this.http.get(this.urlApi + '/api/WorkflowsTemplates/GetByID?ID=' + id);
    }
    public saveDraf(data){
      var token = "Bearer " + localStorage.getItem("token");
      var tokenHeader = new HttpHeaders({'Authorization': token}); 
      return this.http.put(this.urlApi + '/api/WorkflowsTemplates/SaveCraft', data , {headers : tokenHeader });
    }
    public saveWorkFlow(data){
      var token = "Bearer " + localStorage.getItem("token");
      var tokenHeader = new HttpHeaders({'Authorization': token}); 
      return this.http.put(this.urlApi + '/api/WorkflowsTemplates/SaveWorkflow', data , {headers : tokenHeader });
    }
    public checkConnectionWF(data){
      var token = "Bearer " + localStorage.getItem("token");
      var tokenHeader = new HttpHeaders({'Authorization': token}); 
      return this.http.put(this.urlApi + '/api/WorkFlowTemplateActionConnections/CheckConnectionv2', data , {headers : tokenHeader });
    }
    

}
