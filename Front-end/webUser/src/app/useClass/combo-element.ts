export class ComboElement {
    constructor(){
        this.textOnly={name:""},
        this.shortText= {name: "", value: ""},
        this.longText = {name: "", value: ""},
        this.comboBox = {name: "", value: "", valueOfProper: []},
        this.inputCheckbox = {name: "", value: ""}
    };
    textOnly: {name: string};
    shortText: {name: string, value: string};
    longText: {name: string, value: string};
    comboBox: {name: string, value: string, valueOfProper: any};
    inputCheckbox: {name: string, value: string};
    
}

