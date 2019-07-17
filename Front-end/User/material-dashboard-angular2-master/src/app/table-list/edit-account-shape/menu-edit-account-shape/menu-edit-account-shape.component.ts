import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-menu-edit-account-shape',
  templateUrl: './menu-edit-account-shape.component.html',
  styleUrls: ['./menu-edit-account-shape.component.scss']
})
export class MenuEditAccountShapeComponent implements OnInit {
  @Output() draw = new EventEmitter<any[]>();

  public menuList = [
    {
      class: 'example-box aqua-gradient',
      id: '',
      idInput: 'input1',
      idText: '',
      start: true,
      end: false
    },
    {
      class: 'example-box1 purple-gradient',
      id: '',
      idInput: 'input2',
      idText: '',
      start: false,
      end: false
    },
    {
      class: 'example-box3 aqua-gradient',
      id: '',
      idInput: 'input1',
      idText: '',
      start: false,
      end: true
    }
    // {
    //   class: 'example-box2 peach-gradient',
    //   id: '',
    //   idInput: 'input3',
    //   idText: ''
    // },
    // {
    //   class: 'example-box3 blue-gradient',
    //   id: '',
    //   idInput: 'input4',
    //   idText: ''
    // },
    // {
    //   class: 'example-box4',
    //   id: '',
    //   idInput: 'input5',
    //   idText: ''
    // },
  ];

  constructor() { }

  ngOnInit() {
  }

  public drop(event: any) {
    const subEvent = event;
    subEvent.id = '';
    // Bắt Output cho component cha
   this.draw.emit(subEvent);
  }

}
