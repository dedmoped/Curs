import { Component, OnInit } from '@angular/core';
import { from, Subscription } from 'rxjs';
import { Slots } from 'src/app/models/slot';
import {SlotstoreService} from 'src/app/services/slotstore.service'
import {ActivatedRoute} from '@angular/router'
import { FooterRowOutlet } from '@angular/cdk/table';
@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.scss']
})
export class UpdateComponent implements OnInit {

  constructor(private bs:SlotstoreService,private route:ActivatedRoute) 
  { 
this.subscription=route.params.subscribe(params=>this.id=params['id']);
  }
  slots: Slots[]
  changes:Slots = new Slots();
  id:number;
  private subscription:Subscription;

  files: any = [];
  imgUrl:any=[];
   file:any;
  
  ngOnInit(): void {

    this.bs.getOrderById(this.id).subscribe(res=>{this.slots=res})
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
  updateslot(id:number,seller:string,price:number,description:string):void{
   this.changes.id=id;
    this.changes.seller = seller;
    this.changes.cost = price.toString();
   this.changes.description=description;
   this.bs.updateSlot(this.changes,this.files[0]).subscribe();
  }
  carouselOptions = {
    margin: 25,
    nav: true,
    center: true,
    navText: ["<div class='nav-btn prev-slide'></div>", "<div class='nav-btn next-slide'></div>"],
    responsiveClass: true,
    items: 1
  }
}
