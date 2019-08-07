import { Component, OnInit, Inject } from '@angular/core';
import { ComboElement } from 'app/useClass/combo-element';
import { LoadStaffAcountService } from 'app/service/load-staff-acount.service';
import { ToastrService } from 'ngx-toastr';
import {ErrorStateMatcher, MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {FormControl, FormGroupDirective, NgForm, Validators} from '@angular/forms';
import {constructor} from 'sizzle';

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}
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

  listComboElement: any = [];
  nameHere: any;
  propertiesThis: any;
  nameOfCb: any;
  listCb: any = [];
  nameForm: any;
  ngOnInit() {
    this.nameForm="";
    this.nameHere = "";
    this.propertiesThis = 0;
    this.nameOfCb = "";
    // let a = new ComboElement();

    //   a.inputCheckbox.name = "ádsad";

    // this.listComboElement.push(a);
    // console.log(this.listComboElement);

  }

  addProperties(name, properties) {

    let a = new ComboElement();
    debugger;
    if (name == "" || properties == 0) {
      this.toastr.error("Please add name and type for property first.");
    } else {
      if (properties == 1) {
        a.textOnly.name = name;
        a.textOnly.class = "textOnlyClass";
        this.listComboElement.push(a);
      }
      if (properties == 2) {
        a.shortText.name = name;
        a.shortText.class = "shortTextClass";
        this.listComboElement.push(a);

      }
      if (properties == 3) {
        a.longText.name = name;
        a.longText.class = "longTextClass";
        this.listComboElement.push(a);
      }
      if (properties == 4) {
        a.comboBox.name = name;
        if (this.listCb.length == 0) {
          this.toastr.error("Please add option for combo box first.");
        } else {
          this.listCb.forEach(element => {
            a.comboBox.valueOfProper.push(element);
          });
          this.listCb = [];
          this.listComboElement.push(a);

        }
      }
      if (properties == 5) {
        a.inputCheckbox.name = name;
        this.listComboElement.push(a);
      }

    }
  }

  deleteThisRow(index) {
    this.listComboElement.splice(index, 1);
  }
  addToCb(nameOfCb) {
    if (nameOfCb == "") {
      this.toastr.error("Please add a valid option!");
    } else {
      this.listCb.push(nameOfCb);
      this.nameOfCb = "";
      this.toastr.success("Add option success");
    }
  }
  createNewForm() {

    if (this.listComboElement.length == 0) {
      this.toastr.error("Please add some properties for dynamic form!");
    } if(this.nameForm == ""){
      this.toastr.error("Please input name of form!");
    } else {
      let model = {
        "name": this.nameForm,
        "data": JSON.stringify(this.listComboElement)
      }
      this.loadStaffAcountService.createAction(model).toPromise().then(data => {
        this.toastr.success('Success! ', '');
        this.dialogRef.close();
      }, (err) => {
        this.toastr.error("Error:" + err.error, "Something wrong!");
      });
    }
  }
  deleteThisOption(j) {
    this.listCb.splice(j, 1);
  }
  validateInput = new FormControl('', [Validators.required]);
  validatePro = new FormControl('', [Validators.required]);
  addobtion = new FormControl('', [Validators.required]);
  selectFormControl = new FormControl('', Validators.required);
  // tslint:disable-next-line:member-ordering




  // tslint:disable-next-line:member-ordering
  matcher = new MyErrorStateMatcher();
}
