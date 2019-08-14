import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';


@Component({
  selector: 'app-add-manage-per-gr',
  templateUrl: './add-manage-per-gr.component.html',
  styleUrls: ['./add-manage-per-gr.component.scss']
})
export class AddManagePerGrComponent implements OnInit {

  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};

  groupList: any = [];
  permissionList: any = [];
  nameGR: any;
  createGroup = true;
  formData = {groupID:"", permissionIDs: [] };
  saveData: any;
  saveData1: any;

  // formDataEdit = new AddGroupIdName();
  constructor(@Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddManagePerGrComponent>, private toastr: ToastrService,
    private router: Router, private loadStaffAcountService: LoadStaffAcountService) { }

  ngOnInit() {
    this.nameGR = this.data.name;
    console.log("data bên nhỏ:", this.data);
    console.log("name:", this.nameGR);

    
    this.loadStaffAcountService.loadPermissionData().toPromise().then(res => {
      //console.log(res);
      this.permissionList = res;
      this.dropdownList = [];
      this.formData.permissionIDs=[];

      this.permissionList.forEach(element => {
        this.dropdownList.push({ id: element.id, name: element.name }); 
      });
      this.dropdownSettings = {
        singleSelection: false,
        idField: 'id',
        textField: 'name',
        selectAllText: 'Select All',
        unSelectAllText: 'UnSelect All',
        itemsShowLimit: 5,
        allowSearchFilter: true,
      };


      
      this.loadStaffAcountService.loadPermissionByGr(this.data.id).toPromise().then(res2 => {
        this.saveData = res2;
        let listPerNow : any=[];
        this.saveData.forEach(element => {
          listPerNow.push({ id: element.permissionID, name: element.permissionName});
        });
        this.formData.permissionIDs = listPerNow;
      },err =>{
        //this.toastr.error(err.error);
      })


    },err =>{
      this.toastr.error(err.error);
    })

    // debugger;
    // if (this.data != null && this.data != "null") this.createGroup = false;

    // this.loadStaffAcountService.loadGroupData().toPromise().then(data=>{
    //   this.groupList = data;
    //   this.loadStaffAcountService.loadPermissionData().toPromise().then(res=>{
    //     this.permissionList = res;
    //   })
    // })

    // if (!this.createGroup) {
    //   this.loadStaffAcountService.loadPermissionByID(this.data).toPromise().then(res => {
    //     this.saveData = res;
    //   //  this.formData.name = this.saveData.name;
    //   })
    // }
  }
  onItemSelect(item: any) {
    // console.log(item);
    // console.log("select", this.selectedItems);
    // console.log("abc ", this.formData.permissionIDs);
  }
  onSelectAll(items: any) {
   // console.log(items);
  }
  onSubmit() {
      let perListID: any=[];
      this.formData.permissionIDs.forEach(element => {
        perListID.push(element.id);
      });
      let model = {
        groupID: this.data.id,
        permissionIDs: perListID
        
      }
      console.log(JSON.stringify(model));
      this.loadStaffAcountService.putPerGr(model).toPromise().then(
        resp => {
          console.log(resp.toString());
          this.toastr.success("Success");
          this.dialogRef.close()
        }, err =>{
           this.toastr.error(err.error, "Some thing wrong" );
        });



  }

}
