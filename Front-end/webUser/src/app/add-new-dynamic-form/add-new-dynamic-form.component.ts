import { Component, OnInit, Inject } from '@angular/core';
import { ComboElement } from 'app/useClass/combo-element';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { ToastrService } from 'ngx-toastr';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-add-new-dynamic-form',
  templateUrl: './add-new-dynamic-form.component.html',
  styleUrls: ['./add-new-dynamic-form.component.scss']
})
export class AddNewDynamicFormComponent implements OnInit {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data,
    public dialogRef: MatDialogRef<AddNewDynamicFormComponent>,
    private toastr: ToastrService,
    private loadStaffAcountService: LoadStaffAcountService) { }

  listComboElement: any=[];
  nameHere: any;
  propertiesThis: any;
  nameOfCb:any;
  listCb: any =[];
  nameForm: any;
  ngOnInit() {
    // let a = new ComboElement();

    //   a.inputCheckbox.name = "ádsad";
    
    // this.listComboElement.push(a);
    // console.log(this.listComboElement);

  }

  addProperties(name,properties){
    console.log(this.nameHere);
    debugger;
      let a = new ComboElement();
      if(properties == 1){
        a.textOnly.name = name;
      }
      if(properties == 2){
        a.shortText.name = name;
      }
      if(properties == 3){
        a.longText.name = name;
      }
      if(properties == 4){
        a.comboBox.name = name;
        this.listCb.forEach(element => {
          a.comboBox.valueOfProper.push(element);
        });
        this.listCb = [];
      }
      if(properties == 5){
        a.inputCheckbox.name = name;
      }
      this.listComboElement.push(a);
      console.log(this.listComboElement);
  }

  deleteThisRow(index){
    this.listComboElement.splice(index,1);
  }
  addToCb(nameOfCb){
    debugger;
    this.listCb.push(nameOfCb);
    this.toastr.success("Add option success");
  }
  createNewForm(){
    let model = {
      "name": this.nameForm,
      "data": JSON.stringify(this.listComboElement)
    }
    this.loadStaffAcountService.createAction(model).toPromise().then(data =>{
      this.toastr.success('Success! ' , '' );
      this.dialogRef.close();
    }, (err) => {
        this.toastr.error("Error:" + err.message, "Something wrong!" );
      });
  } 

}
