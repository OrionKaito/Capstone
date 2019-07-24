import { Component, OnInit } from '@angular/core';
import { ComboElement } from 'app/useClass/combo-element';

@Component({
  selector: 'app-add-new-dynamic-form',
  templateUrl: './add-new-dynamic-form.component.html',
  styleUrls: ['./add-new-dynamic-form.component.scss']
})
export class AddNewDynamicFormComponent implements OnInit {

  constructor() { }

  listComboElement: any=[];
  nameHere: any;
  propertiesThis: any;
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
        a.inputFile.name = name;
      }
      if(properties == 5){
        a.inputCheckbox.name = name;
      }
      this.listComboElement.push(a);
      console.log(this.listComboElement);

  }

}
