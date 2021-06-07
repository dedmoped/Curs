import { Component, OnInit } from '@angular/core';
import { from, Subscription } from 'rxjs';
import { Slots } from 'src/app/models/slot';
import {SlotstoreService} from 'src/app/services/slotstore.service'
import {ActivatedRoute} from '@angular/router'
import { FooterRowOutlet } from '@angular/cdk/table';
import { ToastrService } from 'ngx-toastr';
import { title } from 'process';
@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.scss']
})
export class UpdateComponent implements OnInit {

  constructor(private bs: SlotstoreService, private route: ActivatedRoute, private Toast: ToastrService) 
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
    this.files = [];
    this.imgUrl = [];
    for (let index = 0; index < event.length; index++) {
      const element = event[index];
      this.files[index] = element
      var reader = new FileReader();
      reader.onload = (_event) => {
        this.imgUrl[index] = _event.target.result;
      }
      reader.readAsDataURL(element);
    }
  }




  updateslot(id:number,title:string,price:number,description:string):void{
   this.changes.id=id;
    this.changes.title = title
    this.changes.cost = price.toString();
    this.changes.description = description;
    this.bs.updateSlot(this.changes, this.files).subscribe(res => this.Toast.success(res["message"]), err => { this.Toast.error(err.error.text)});
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
