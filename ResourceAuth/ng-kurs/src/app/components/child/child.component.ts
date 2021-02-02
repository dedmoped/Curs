import { Component, OnInit } from '@angular/core';
import { from, Subscription } from 'rxjs';
import { Slots } from 'src/app/models/slot';
import {SlotstoreService} from 'src/app/services/slotstore.service'
import {ActivatedRoute} from '@angular/router'

@Component({
  selector: 'app-child',
  templateUrl: './child.component.html',
  styleUrls: ['./child.component.scss']
})
export class ChildComponent implements OnInit {

  constructor(private bs:SlotstoreService) 
  { 
  }
  slots: Slots[]
  changes:Slots = new Slots();
  id:number;
  private subscription:Subscription;

  files: any = [];
  imgUrl:any=[];
   file:any;
  
  ngOnInit(): void {

  }
  
  uploadFile(event) {
    for (let index = 0; index < event.length; index++) {
      const element = event[index];
      this.files[0]=element
      var reader= new FileReader();
      reader.readAsDataURL(element);
      reader.onload=(_event)=>{
      this.imgUrl[0]=reader.result;
     }
      
  }
}
  updateslot():void{
    }
}
