import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
// import $ from 'jquery';
import * as $ from 'jquery';
import 'jquery-ui/ui/widgets/draggable.js';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { saveAs } from 'file-saver';
import * as moment from 'moment-timezone';
import { interval } from 'rxjs';
import { ModalDirective } from 'angular-bootstrap-md';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ShapeService } from './shape.service';
import { ActivatedRoute } from '@angular/router';
import { element } from '@angular/core/src/render3';
import { AddNewDynamicFormComponent } from 'app/add-new-dynamic-form/add-new-dynamic-form.component';

@Component({
  selector: 'app-edit-account-shape',
  templateUrl: './edit-account-shape.component.html',
  styleUrls: ['./edit-account-shape.component.scss']
})
export class EditAccountShapeComponent implements OnInit {
  @ViewChild('basicModal') public showModalOnClick: ModalDirective;

  properties: any;
  propertiesArr: any;
  checkIsAction: boolean;
  checkIsArrow: boolean;
  listActionType: any = [];
  listConnectionType: any = [];

  public dataModel: any;

  secondFormGroup: FormGroup;

  listDropdown = [];

  public isLoading = true;
  public drawImage: any;
  public subHeight = 0;
  // Tạo height động cho thẻ svg
  public heightSVG = '100vh';


  public subClass = [];
  public classQuerySelector = [];

  public listClass: any = [];


  public menuList1 = [];
  public subMenu = [];

  public listArrow = [];
  public listdivA = [];
  public listdivB = [];

  public posnALeft = [];
  public posnBLeft = [];

  public arrowLeft = [];

  public listInput = [];

  public fileContent: any;
  public positionTop: any;
  public positionLeft: any;
  public enableDeleteArrow = false;
  saveIDofWF:any;

  getListPer: any;
  getListCon: any;
  getListAction: any;
  constructor(
    public toastr: ToastrService,
    public dialog: MatDialog,
    private shapeService: ShapeService,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
  ) { }
  saveActiontype;
  ngOnInit() {
    this.properties = {
      name: "",
      description: "",
      permissionToUseID: "0",
      isApprovedByLineManager: false,
    }
    this.propertiesArr = {
      name: ""
    }




    this.activatedRoute.params.subscribe(item => {
      if (item.id) {
        this.saveIDofWF = item.id;
        //load dữ liệu của thằng này kiểu json để show lên lại
        this.shapeService.getJsonByUserId(item.id).toPromise().then(res => {

          if (res) {
            this.saveActiontype = res;
            var b = this.saveActiontype.data;
            console.log(b);
            var a = JSON.parse(this.saveActiontype.data);
            console.log(a);
            // var c = a.json;
            console.log("thu nhat");
            this.addImportEnd(a);
            console.log("ádasd");
          }
        });
      }
    });
    this.getDropdownList();
    this.getlistActionType();
    this.getListConnectionType();
  }
  //lấy list permission
  public getDropdownList() {
    this.shapeService.getDropdownList().toPromise().then((res: any) => {
      if (res) {
        this.getListPer = res;
        this.getListPer.forEach(item => {
          this.listDropdown.push(item);
        });
      }

    })
  }
  public getListConnectionType(){
    this.shapeService.getConnectionType().toPromise().then((res: any) => {
      if (res) {
        this.getListCon = res;
        this.getListCon.forEach(item => {
          this.listConnectionType.push(item);
        });
      }

    })
  }
   public getlistActionType(){
    this.shapeService.getActiontype().toPromise().then((res: any) => {
      debugger;
      if (res) {
        this.getListAction = res;
        this.getListAction.forEach(item => {
          this.listActionType.push(item);
        });
      }

    })
   }
  // hình như là để vẽ mũi tên, k chắc lắm, hình như đúng rồi, haha
  //item này là của menuList1, menuList1 hình như là để chứa các hình
  public dropImageBottom(item) {
    //lấy id của hình ra, bỏ vô subClass
    this.subClass.push(item.id);
    //cả đoạn này chỉ để đặt tên cho mũi tên + ghi id  div kết nối
    if (this.subClass.length > 1) {
      let count: any;
      //mũi tên đầu dặt tên là arrow0
      if (this.listArrow.length <= 0) {
        count = 0;
        this.listClass.push({
          idDiv: this.subClass,
          idArrow: this.create_UUID().toString(),
          name: 'arrow' + count

        });
      } else {
        //những mũi tên tiếp theo nó lấy mũi đầu cắt chuỗi lấy số rồi cộng lên
        const subCount = this.listArrow[this.listArrow.length - 1];
        //count = subCount.split('arrow');
        count = this.listArrow.length;
        this.listClass.push({
          // idDiv: 2 id của hai điểm
          idDiv: this.subClass,
          // idArrow: 2 điểm có một mủi tên
          // idArrow: 'arrow' + this.listClass.length
          idArrow: this.create_UUID().toString(),
          name: 'arrow' + (+count + 1)
        });
      }
      // Gọi hàm vẽ mủi tên
      //(chl) cái tham số bottom này chưa hiểm lắm
      this.draw('bottom');
    }
  }

  //sau khi click hình bên trái thì tạo 1 hình phía bên phải (chl)
  public drop(event) {
    const subEvent = event;
    const count = this.menuList1.length;

    // Tạo id động cho từng hình
    subEvent.id = this.create_UUID().toString();
    subEvent.name = 'FormName' + count;
    subEvent.description = 'FormDescription' + count;
    subEvent.permissionToUseID = "";
    subEvent.isApprovedByLineManager = false;
    //subEvent.idText = 'form' + count;
    subEvent.formControlName = 'form' + count;
    subEvent.actionTypeID = "";
    // subEvent.dropdown = this.listDropdown;

    debugger;

    // thêm vô 2 cái list
    this.menuList1.push(JSON.parse(JSON.stringify(subEvent)));
    this.subMenu.push(JSON.parse(JSON.stringify(subEvent)));

    if (subEvent.formControlName === 'form0') {
      const abc = {};
      abc[subEvent.formControlName] = [''];
      this.secondFormGroup = this.formBuilder.group(abc);
    }

    //hình như thằng ni là thằng để sinh ra
    this.menuList1.forEach(item => {
      this.secondFormGroup.addControl(item.formControlName, new FormControl(''));
    })

    // Delay để gọi hàm Drag của Jquery
    setTimeout(() => {
      this.initDraw();
    }, 500);

    // Show message
    if (this.menuList1.length < 2) {
      this.toastr.info('Di chuyển hình !!', '', { timeOut: 5000 });
      setTimeout(() => {
        this.toastr.info('Nhấn delete để xóa !!', '', { timeOut: 5000 });
      }, 2000);
    }

  }

  //import json file từ máy
  public importJson(fileList: FileList): void {
    const file = fileList[0];
    if (file.type !== 'application/json') {
      this.toastr.error('Lỗi định dạng !!');
    } else {
      this.menuList1 = [];
      this.listArrow = [];
      const fileReader: FileReader = new FileReader();
      const that = this;
      fileReader.onloadend = function (x) {
        that.fileContent = fileReader.result;
        that.addImportEnd(JSON.parse(that.fileContent));
      };
      fileReader.readAsText(file);
    }
  }
  //chuyển file json thành hình
  public addImportEnd(dataImport: any) {
    console.log(JSON.stringify(dataImport));
    this.menuList1 = [];
    this.listArrow = [];
    const subData = [];
    // if (dataImport.length <= 0) {
    //   this.toastr.error('Lỗi định dạng !!');
    // } else {
    //   // this.progress.show();
    //   //lấy ra thôi, do để json dạng mảng nên ms lấy lenght rồi trừ rắc rối ri
    //   //cả đoạn này chỉ để check id
    //   for (let i = 0; i < (dataImport.length - 1); i++) {
    //     // xem có trùng thằng nào k
    //     const exitKey = this.isExistImport(subData, dataImport[i].key[0][0].id);
    //     // nếu k trùng thì push vô list
    //     if (exitKey === -1) {
    //       subData.push(dataImport[i].key[0][0]);
    //     }
    //     for (let e = 0; e < dataImport[i].option.length; e++) {
    //       const exitOption = this.isExistImport(subData, dataImport[i].option[e][0].id);
    //       if (exitOption === -1) {
    //         subData.push(dataImport[i].option[e][0]);
    //       }
    //     }
    //   }
    // }


    if (dataImport.action.length <= 0) {
      this.toastr.error('Lỗi định dạng !!');
    } else {
      // this.progress.show();
      //lấy ra thôi, do để json dạng mảng nên ms lấy lenght rồi trừ rắc rối ri
      //cả đoạn này chỉ để check id
      for (let i = 0; i < dataImport.action.length; i++) {
        // xem có trùng thằng nào k
        const exitKey = this.isExistImport(subData, dataImport.action[i].id);
        // nếu k trùng thì push vô list
        if (exitKey === -1) {
          subData.push(dataImport.action[i]);
        }
      }
    }


    // thằng này là lấy chữ form1,2,3 chi đó ra, nhưng chưa hieru lắm để làm j
    //hình như để gọi thư viện hổ trợ sinh ra
    const abc = {};
    abc[subData[0].formControlName] = [''];
    this.secondFormGroup = this.formBuilder.group(abc);

    for (let i = 0; i < subData.length; i++) {
      this.secondFormGroup.addControl(subData[i].formControlName, new FormControl(''));
      this.secondFormGroup.get(subData[i].formControlName).setValue(subData[i].value);
    }

    //gắn vô 2 bién
    this.menuList1 = subData;
    this.listClass = dataImport.arrow;
    if (this.menuList1.length <= 0) {
      this.toastr.error('Lỗi định dạng !!');
    } else {
      setTimeout(() => {
        this.initDraw();
        this.draw('importJSON');
        // this.progress.hide();
      }, 500);
    }
  }


  //xuất file json
  public exportJson() {
    let dataKey: any;
    let dataOption: any;
    let positionKey: any;
    let positionOption: any;

    let classKey: any;
    let classOption: any;

    //const exportJson = [];
    let exportJson: any = { action: [], arrow: [] };
    let exportJson2: any = {workFlowID: String, action: [], arrow: [] };
    // for (let i = 0; i < this.listClass.length; i++) {
    //   let obj;
    //   // Lấy value từ id input tag
    //   // dataKey = $('#' + this.listClass[i].idDiv[0].replace('a', 'form').toString())[0].value;
    //   const subControl = this.listClass[i].idDiv[0].replace('a', 'form');
    //   dataKey = this.secondFormGroup.controls[subControl].value;
    //   positionKey = $('#' + this.listClass[i].idDiv[0]);
    //   classKey = this.menuList1.filter(item => item.id === this.listClass[i].idDiv[0]);
    //   classKey[0].positionTop = positionKey.position().top;
    //   classKey[0].positionLeft = positionKey.position().left;
    //   classKey[0].value = dataKey;
    //   // dataOption = $('#' + this.listClass[i].idDiv[1].replace('a', 'form').toString())[0].value;
    //   const subControlOption = this.listClass[i].idDiv[1].replace('a', 'form');
    //   dataOption = this.secondFormGroup.controls[subControlOption].value;
    //   positionOption = $('#' + this.listClass[i].idDiv[1]);
    //   classOption = this.menuList1.filter(item => item.id === this.listClass[i].idDiv[1]);
    //   classOption[0].positionTop = positionOption.position().top;
    //   classOption[0].positionLeft = positionOption.position().left;
    //   classOption[0].value = dataOption;
    //   const index = this.isExist(exportJson, dataKey);
    //   if (index === -1) {
    //     obj = {
    //       key: [
    //         classKey
    //       ],
    //       option: [
    //         classOption
    //       ]
    //     };
    //   } else {
    //     (exportJson[index].option).push(classOption);
    //   }
    //   if (obj) {
    //     exportJson.push(obj);
    //   }
    // }

    // for (let i = 0; i < this.listClass.length; i++) {
    //   let obj;
    //   //đổi id từ a thành form thôi
    //   const subControl = this.listClass[i].idDiv[0].replace('a', 'form');
    //   // ???(chl)  hình như để lấy value từ input thôi hay là gôm ID lại để check coi  có 2 cái nào trùng nhau k á
    //   dataKey = this.secondFormGroup.controls[subControl].value;
    //   // lấy ra id chính thằng đó để lấy vị trí
    //   positionKey = $('#' + this.listClass[i].idDiv[0]);
    //   // lọc ra những thằng làm đầu mũi tên
    //   classKey = this.menuList1.filter(item => item.id === this.listClass[i].idDiv[0]);
    //   // thêm 2 thuộc tính
    //   classKey[0].positionTop = positionKey.position().top;
    //   classKey[0].positionLeft = positionKey.position().left;
    //   // gắn ngược lại thằng kia để làm gì đây? cảm giác như nó éo làm dc j 
    //   classKey[0].value = dataKey;

    //   // tương tự cho thằng option
    //   const subControlOption = this.listClass[i].idDiv[1].replace('a', 'form');
    //   dataOption = this.secondFormGroup.controls[subControlOption].value;
    //   positionOption = $('#' + this.listClass[i].idDiv[1]);
    //   classOption = this.menuList1.filter(item => item.id === this.listClass[i].idDiv[1]);
    //   classOption[0].positionTop = positionOption.position().top;
    //   classOption[0].positionLeft = positionOption.position().left;
    //   classOption[0].value = dataOption;

    //   //đoạn này bắt đầu thêm vô nè

    //   // check id trùng k?
    //   // mà nó rổng, thì check làm cc j nhỉ?
    //   //kệ cmn đi
    //   const index = this.isExist(exportJson, dataKey);
    //   // đoạn ni thêm cái đầu vô, là mấy cái action á
    //   if (index === -1) {
    //     obj = {
    //       key: [
    //         classKey
    //       ],
    //       option: [
    //         classOption
    //       ]
    //     };
    //   } else {
    //     (exportJson[index].option).push(classOption);
    //   }
    //   if (obj) {
    //     exportJson.push(obj);
    //   }


    // }





    for (let i = 0; i < this.menuList1.length; i++) {
      let obj;



      positionKey = $('#' + this.menuList1[i].id);
      // lọc ra những thằng làm đầu mũi tên
      classKey = this.menuList1[i];
      // thêm 2 thuộc tính
      classKey.positionTop = positionKey.position().top;
      classKey.positionLeft = positionKey.position().left;
      exportJson.action.push(classKey);
      exportJson2.action.push(classKey);


    }
    if (exportJson.length <= 0) {
      this.toastr.error('Chưa có dữ liệu !!');
    } else
    // if (dataKey === '' || dataOption === '') {
    //   this.toastr.error('Vui lòng nhập dữ liệu !!');
    // } else 
    {
      //đẩy mũi tên vô có 1 dòng vậy thôi à?
      this.listClass.forEach(element => {
        exportJson.arrow.push(element);
        exportJson2.arrow.push(element);
      })

      exportJson2.workFlowID= this.saveIDofWF;
      //lưu  lại
      const json = JSON.stringify(exportJson2);
      const blob = new Blob([json], { type: 'application/json' });
      saveAs(blob, 'data' + this.convertDateFileName(new Date()) + '.json');
      //đoạn này mình chỉnh sửa để push dc lên server
      // const data = {
      //   json: exportJson
      // }
      var a = {id: this.saveIDofWF, "data": JSON.stringify(exportJson)};
      debugger;
      // this.shapeService.postJsonFile(a).subscribe((res: any) => {
      // }, (err) => {
      //   console.log(err);
      // });
      exportJson2.workFlowID= this.saveIDofWF;
      var b = {id: this.saveIDofWF, "data": JSON.stringify(exportJson2)};
      // this.shapeService.saveAndACtiveJsonFile(exportJson2).subscribe((res: any) => {
      // }, (err) => {
      //   console.log(err);
      // });
    }
  }

  isExistImport(exportJson, key) {
    let index = 0;
    for (const item of exportJson) {
      if (item && item.id === key.toString()) {
        return index;
      }
      index++;
    }
    return -1;
  }

  // Kiếm tra tồn tại của key
  isExist(exportJson, key) {
    let index = 0;
    for (const item of exportJson) {
      if (item && item.key[0][0].value === key.toString()) {
        return index;
      }
      index++;
    }
    return -1;
  }

  // Lấy tên file theo ngày giờ
  public convertDateFileName(date: Date): string {
    const value = moment(date);
    return value.format('_YYYYMMDD' + 'hhmmss');
  }


  //hàm này để vẽ, nhưng chưa biết là vẽ gì, là vẽ mũi tên đó à, ahihi
  public draw(location) {
    // Có đủ hai điểm để vẽ mủi tên
    if (this.subClass.length > 1) {
      this.subClass = [];
      this.classQuerySelector = [];
      this.listArrow = [];
      this.posnALeft = [];
      this.posnBLeft = [];
      for (let i = 0; i < this.listClass.length; i++) {
        this.listArrow.push(this.listClass[i].idArrow);
        const subClassQuerySelector = [];
        for (let e = 0; e < this.listClass[i].idDiv.length; e++) {
          // Push querySelector id của hai hình để lấy vị trí
          subClassQuerySelector.push(document.querySelector('#' + this.listClass[i].idDiv[e]));
        }
        this.classQuerySelector.push(subClassQuerySelector);
      }
      // Chạy for lấy 2 vị trí x và y của hai hình để xác định vị trí vẽ mủi tên
      for (let i = 0; i < this.classQuerySelector.length; i++) {
        for (let e = 0; e < this.classQuerySelector[i].length; e++) {
          if (e === 0) {
            // Vị trí x, y hình 1
            this.posnALeft.push({
              x: this.classQuerySelector[i][e].offsetLeft - 5,
              y: this.classQuerySelector[i][e].offsetTop + (this.classQuerySelector[i][e].offsetHeight / 2)
            });
          } else {
            // Vị trí x, y hình 2
            this.posnBLeft.push({
              x: this.classQuerySelector[i][e].offsetLeft - 5,
              y: this.classQuerySelector[i][e].offsetTop + (this.classQuerySelector[i][e].offsetHeight / 2)
            });
          }
        }
      }
      this.arrowLeft = [];
      // Chạy for cho nhiều mủi tên của nhiều cặp hình
      for (let i = 0; i < this.listArrow.length; i++) {
        let subArrow: any;
        // interval 0,5s để html kịp zen mủi tên
        const source = interval(500).subscribe(() => {
          // lấy querySelector của mủi tên
          subArrow = document.querySelector('#' + this.listArrow[i]);
          if (subArrow) {
            this.arrowLeft.push(document.querySelector('#' + this.listArrow[i]));
            source.unsubscribe();
            for (let e = 0; e < this.arrowLeft.length; e++) {
              // Tạo vị trí của mủi tên
              const dStrLeft =
                // 'M' bắt đầu vị trí mủi tên là vị trí x, y của hình 1
                // https://www.w3.org/TR/SVG/paths.html
                'M' +
                (this.posnALeft[e].x + 55) + ',' + (this.posnALeft[e].y) + ' ' +
                // 'L' vẽ một đường thắng bắt đầu từ điểm 'M' đến điểm x, y của 'L'
                'L' +
                (this.posnBLeft[e].x + 55) + ',' + (this.posnBLeft[e].y);
              // setAttribute để vẽ mủi tên
              this.arrowLeft[e].setAttribute('d', dStrLeft);
            }
          }
        });
      }
    } else if (location === 'importJSON') {
      this.subClass = [];
      this.classQuerySelector = [];
      this.listArrow = [];
      this.posnALeft = [];
      this.posnBLeft = [];
      for (let i = 0; i < this.listClass.length; i++) {
        this.listArrow.push(this.listClass[i].idArrow);
        const subClassQuerySelector = [];
        for (let e = 0; e < this.listClass[i].idDiv.length; e++) {
          // Push querySelector id của hai hình để lấy vị trí
          subClassQuerySelector.push(document.querySelector('#' + this.listClass[i].idDiv[e]));
        }
        this.classQuerySelector.push(subClassQuerySelector);
      }
      // Chạy for lấy 2 vị trí x và y của hai hình để xác định vị trí vẽ mủi tên
      for (let i = 0; i < this.classQuerySelector.length; i++) {
        for (let e = 0; e < this.classQuerySelector[i].length; e++) {
          if (e === 0) {
            // Vị trí x, y hình 1
            this.posnALeft.push({
              x: this.classQuerySelector[i][e].offsetLeft - 5,
              y: this.classQuerySelector[i][e].offsetTop + (this.classQuerySelector[i][e].offsetHeight / 2)
            });
          } else {
            // Vị trí x, y hình 2
            this.posnBLeft.push({
              x: this.classQuerySelector[i][e].offsetLeft - 5,
              y: this.classQuerySelector[i][e].offsetTop + (this.classQuerySelector[i][e].offsetHeight / 2)
            });
          }
        }
      }
      this.arrowLeft = [];
      // Chạy for cho nhiều mủi tên của nhiều cặp hình
      for (let i = 0; i < this.listArrow.length; i++) {
        let subArrow: any;
        // interval 0,5s để html kịp zen mủi tên
        const source = interval(500).subscribe(() => {
          // lấy querySelector của mủi tên
          subArrow = document.querySelector('#' + this.listArrow[i]);
          if (subArrow) {
            this.arrowLeft.push(document.querySelector('#' + this.listArrow[i]));
            source.unsubscribe();
            for (let e = 0; e < this.arrowLeft.length; e++) {
              // Tạo vị trí của mủi tên
              const dStrLeft =
                // 'M' bắt đầu vị trí mủi tên là vị trí x, y của hình 1
                // https://www.w3.org/TR/SVG/paths.html
                'M' +
                (this.posnALeft[e].x + 55) + ',' + (this.posnALeft[e].y) + ' ' +
                // 'L' vẽ một đường thắng bắt đầu từ điểm 'M' đến điểm x, y của 'L'
                'L' +
                (this.posnBLeft[e].x + 55) + ',' + (this.posnBLeft[e].y);
              // setAttribute để vẽ mủi tên
              this.arrowLeft[e].setAttribute('d', dStrLeft);
            }
          }
        });
      }
    }
    // this.progress.hide();
  }

  // Function vẽ lại hình khi drag khi đã vẽ mủi tên
  public subDrag(id) {
    const subListClass = [];
    const subOffsetTop = [];
    if (this.listArrow.length > 0) {
      for (let i = 0; i < this.listClass.length; i++) {
        for (let e = 0; e < this.listClass[i].idDiv.length; e++) {
          const isExit = subListClass.includes(this.listClass[i].idDiv[e]);
          if (!isExit) {
            subListClass.push(this.listClass[i].idDiv[e]);
            const offsetTop: any = document.querySelector('#' + this.listClass[i].idDiv[e]);
            subOffsetTop.push(offsetTop.offsetTop);
          }
        }
      }
      const subHeightSVG = subOffsetTop.sort((a, b) => a > b ? -1 : 1);
      this.heightSVG = subHeightSVG[0] + 100 + 'px';
    }

    const subData = $('#' + id);
    this.positionTop = subData.position().top;
    this.positionLeft = subData.position().left;
    if (this.listArrow.length > 0) {
      const avgDiv = [];
      const subArrow = [];
      this.listClass.forEach(element => {
        element.idDiv.filter(item => {
          if (item === id) {
            subArrow.push(element.idArrow);
            avgDiv.push(element.idDiv);
          }
        });
      });
      const arrowLeft = [];
      for (let i = 0; i < subArrow.length; i++) {
        const divA: any = document.querySelector('#' + avgDiv[i][0]);
        const divB: any = document.querySelector('#' + avgDiv[i][1]);
        let subArrow1: any;
        const source = interval(500).subscribe(() => {
          subArrow1 = document.querySelector('#' + subArrow[i]);
          if (subArrow1) {
            arrowLeft.push(document.querySelector('#' + subArrow[i]));
            source.unsubscribe();
            const posnALeft = {
              x: divA.offsetLeft - 5,
              y: divA.offsetTop + (divA.offsetHeight / 2)
            };
            const posnBLeft = {
              x: divB.offsetLeft - 5,
              y: divB.offsetTop + (divB.offsetHeight / 2)
            };
            const dStrLeft =
              'M' +
              (posnALeft.x + 55) + ',' + (posnALeft.y) + ' ' +
              'L' +
              (posnBLeft.x + 55) + ',' + (posnBLeft.y);
            arrowLeft[i].setAttribute('d', dStrLeft);
          }
        });
      }
    }
  }

  // Function apply draggable cho tất cả hình
  public initDraw() {

    const that = this;
    const id = [];
    let subId = '';

    // Push từng id riêng biệt cho hình
    for (let i = 0; i < this.menuList1.length; i++) {
      id.push('#' + this.menuList1[i].id);
    }
    for (let e = 0; e < id.length; e++) {
      (e + 1) === id.length ? subId += id[e] : subId += id[e] + ',';
    }
    //chien: chac la ho tro noi sau khi ve, thêm hình
    //khi ve thi goi cai thang ve lai
    $(subId).draggable({
      drag: function (event, ui) {
        const idDrag = event.target.id;
        that.subDrag(idDrag);
      }
    });
  }

  public clearAll() {
    try {
      this.menuList1 = [];
      this.listArrow = [];
      this.listClass = [];
      this.enableDeleteArrow = false;
      this.toastr.success('Xóa tất cả thành công !!');
    } catch (error) {
      this.toastr.error('Xóa tất cả thất bại !!');
    }
  }

  public openDialog(item) {
    debugger;
    this.listClass.forEach(element => {
      if (element.idArrow === item) {
        this.propertiesArr = element;
        this.checkIsAction = false;
        this.checkIsArrow = true;
      }
    });
    if (localStorage.getItem('arrow') == item) {
      this.showModalOnClick.show();
      this.enableDeleteArrow = true;
    } else {
      localStorage.setItem('arrow', item);
    }

  }

  public clickArrow() {
    this.showModalOnClick.hide();
    const item = localStorage.getItem('arrow');
    if (this.enableDeleteArrow === true) {
      for (let i = 0; i < this.listArrow.length; i++) {
        if (this.listArrow[i] === item) {
          this.listArrow.splice(i, 1);
          this.listClass.splice(i, 1);
        }
      }
    }
    localStorage.clear();
  }


  //cái ni để nhấn nút start vs end
  public startShape(item) {
    this.menuList1.forEach(element => {
      if (element.id === item.id) {
        element.start = element.start;
      } else {
        element.start = !element.start;
      }
    });
  }

  showProperties(item) {
    debugger;
    this.menuList1.forEach(element => {
      if (element.id === item.id) {
        this.properties = element;
        this.checkIsAction = true;
        this.checkIsArrow = false;
      }
    });
  }
  public endShape(item) {
    this.menuList1.forEach(element => {
      if (element.id === item.id) {
        element.end = element.end;
      } else {
        element.end = !element.end;
      }
    });
  }

  // Bắt sự kiện nút delete để xóa hình
  // Sẽ bắt sự kiện xóa mủi tên sau
  @HostListener('window:keyup', ['$event'])
  keyEvent(event: KeyboardEvent) {
    if (event.keyCode === 46) {
      this.menuList1.pop();
      // this.listArrow.pop();
      // this.listClass.pop();
    }
  }

  openDinamicForm(id){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.width = "50%";
    dialogConfig.data = id;
    this.dialog.open(AddNewDynamicFormComponent, dialogConfig).afterClosed().subscribe(res => {
      console.log(res);
    });

  }
  saveDraf(){
    let positionKey: any;
    let classKey: any;

    //const exportJson = [];
    let exportJson: any = { action: [], arrow: [] };
    let exportJson2: any = {workFlowID: String, action: [], arrow: [] };

    for (let i = 0; i < this.menuList1.length; i++) {
      let obj;
      positionKey = $('#' + this.menuList1[i].id);
      // lọc ra những thằng làm đầu mũi tên
      classKey = this.menuList1[i];
      // thêm 2 thuộc tính
      classKey.positionTop = positionKey.position().top;
      classKey.positionLeft = positionKey.position().left;
      exportJson.action.push(classKey);
      exportJson2.action.push(classKey);


    }
    if (exportJson.length <= 0) {
      this.toastr.error('Chưa có dữ liệu !!');
    } else
    {
      //đẩy mũi tên vô có 1 dòng vậy thôi à?
      this.listClass.forEach(element => {
        exportJson.arrow.push(element);
        exportJson2.arrow.push(element);
      })

      exportJson2.workFlowID= this.saveIDofWF;
      //lưu  lại
      const json = JSON.stringify(exportJson2);


      var a = {"id": this.saveIDofWF, "data": JSON.stringify(exportJson)};
      exportJson2.workFlowID= this.saveIDofWF;
      var b = {id: this.saveIDofWF, "data": JSON.stringify(exportJson2)};

      this.shapeService.saveDraf(a).subscribe((res: any) => {
      }, (err) => {
        console.log(err);
      });
    }
  }
  saveWorkFlow(){
    let positionKey: any;
    let classKey: any;

    //const exportJson = [];
    let exportJson: any = { action: [], arrow: [] };
    let exportJson2: any = {workFlowID: String, action: [], arrow: [] };

    for (let i = 0; i < this.menuList1.length; i++) {
      let obj;
      positionKey = $('#' + this.menuList1[i].id);
      // lọc ra những thằng làm đầu mũi tên
      classKey = this.menuList1[i];
      // thêm 2 thuộc tính
      classKey.positionTop = positionKey.position().top;
      classKey.positionLeft = positionKey.position().left;
      exportJson.action.push(classKey);
      exportJson2.action.push(classKey);


    }
    if (exportJson.length <= 0) {
      this.toastr.error('Chưa có dữ liệu !!');
    } else
    {
      //đẩy mũi tên vô có 1 dòng vậy thôi à?
      this.listClass.forEach(element => {
        exportJson.arrow.push(element);
        exportJson2.arrow.push(element);
      })

      exportJson2.workFlowID= this.saveIDofWF;
      //lưu  lại
      const json = JSON.stringify(exportJson2);

      let jsonConnections : any = [];
      let  jsonActions : any=[];
      exportJson.arrow.forEach(element => {
        let b=  {
          fromWorkFlowTemplateActionID: "",
          toWorkFlowTemplateActionID: "",
          connectionTypeID: ""
        };
        b.fromWorkFlowTemplateActionID = element.idDiv[0].toString();
        b.toWorkFlowTemplateActionID = element.idDiv[1].toString();
        b.connectionTypeID = element.name.toString();
        jsonConnections.push(b);
      });
      exportJson.action.forEach(element => {
        
        if(element.permissionToUseID != "" && element.permissionToUseID != "0" &&
         element.permissionToUseID != null) {
          let oneAction=  {
            id: element.id,
            name: element.name,
            description: element.description,
            actionTypeID: element.actionTypeID,
            permissionToUseID: element.permissionToUseID,
            isApprovedByLineManager: element.isApprovedByLineManager,
            isStart: element.isStart,
            isEnd: element.isEnd
          };
          jsonActions.push(oneAction);
         } else {
          let oneAction=  {
            id: element.id,
            name: element.name,
            description: element.description,
            actionTypeID: element.actionTypeID,
            // permissionToUseID: element.permissionToUseID,
            isApprovedByLineManager: element.isApprovedByLineManager,
            isStart: element.isStart,
            isEnd: element.isEnd
          };
          jsonActions.push(oneAction);
         }

        
      });
      let jsonData = JSON.stringify(exportJson);
      let a = {"workFlowTemplateID": this.saveIDofWF, "data": jsonData,
       "actions": jsonActions, "connections":jsonConnections};

      exportJson2.workFlowID= this.saveIDofWF;
      debugger;
      console.log("file json:")
      console.log(JSON.stringify(a));

      this.shapeService.saveWorkFlow(a).subscribe((res: any) => {
      }, (err) => {
        console.log(err);
      });
    }
  }
  create_UUID(){
    var dt = new Date().getTime();
    var uuid = 'axxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = (dt + Math.random()*16)%16 | 0;
        dt = Math.floor(dt/16);
        return (c=='x' ? r :(r&0x3|0x8)).toString(16);
    });
    return uuid;
}
}
