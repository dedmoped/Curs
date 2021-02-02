import { Component, OnInit } from '@angular/core';
import { from, Observable } from 'rxjs';
import {SlotstoreService} from 'src/app/services/slotstore.service'

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.scss']
})
export class UploadFileComponent implements OnInit {

  constructor( private up: SlotstoreService) { }

  ngOnInit(): void {
  }
  files: any = [];
  imgUrl:any=[];

  uploadFile(event) {
    for (let index = 0; index < event.length; index++) {
      const element = event[index];
      this.files.push(element)
      var reader= new FileReader();
      reader.readAsDataURL(element);
      reader.onload=(_event)=>{
      this.imgUrl[this.files.length-1]=reader.result;
      }
    }  
  }
  deleteAttachment(index) {
    this.files.splice(index, 1)
  }
  sendAttachment(index){
   // this.up.addSlot(this.files[index]).subscribe();
  }


}
