export class ComboElement {
    constructor(){
        this.textOnly={name:"", class:""},
        this.shortText= {name: "", value: "", class:""},
        this.longText = {name: "", value: "", class:""},
        this.comboBox = {name: "", value: "", valueOfProper: [], class:""},
        this.inputCheckbox = {name: "", value: "", class:""}
    };
    textOnly: {name: string, class: string};
    shortText: {name: string, value: string,  class: string};
    longText: {name: string, value: string,  class: string};
    comboBox: {name: string, value: string, valueOfProper: any, class: string;}
    inputCheckbox: {name: string, value: string,  class: string};
    
}

