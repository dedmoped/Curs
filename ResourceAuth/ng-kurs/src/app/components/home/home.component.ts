import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { from, Observable } from 'rxjs';
import { Slots } from 'src/app/models/slot';
import {SlotstoreService} from 'src/app/services/slotstore.service'
import * as $ from 'jquery';
import { element } from 'protractor';
import { FormControl, Validators } from "@angular/forms"
import { AuthService } from '../../services/auth.service';
import { error } from '@angular/compiler/src/util';
import { tap } from 'rxjs/operators';
export const ACCESS_ID = 'slotstore_access_id'
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit,AfterViewInit {
   Slot:Slots[]=[]
   posts: Slots[];
  userRate: [];
  data: any;
  //ctrl= new FormControl(null,Validators.required);
  constructor(private bs: SlotstoreService, private http: HttpClient, private route: Router, private auth: AuthService) { }
   
  ngOnInit(): void {
    this.bs.getCatalog().subscribe(res=>{this.posts=res, this.posts.forEach((currentvalue,index)=>{
      $.ajax({
        url: "http://localhost/api/slots/rate/"+currentvalue.sellerid, success: function(result){
      (<HTMLInputElement>document.getElementById(currentvalue.id.toString())).value=result;     
    },error:function(){alert("errr");}});
    })});
  }
  
  ngAfterViewInit(){
 
     
  }
  deleteslot(id: number) {
    this.bs.deleteslot(id).subscribe(res => {
      
    },
      error=>
      {
        this.ngOnInit();
    }
      );
  }
  
  addorder(id:number):void{
    this.bs.addOrder(id).subscribe();
  }
  updateslot(id:number):void{
    this.route.navigate(["update/"+id]);
  }
  slotinfo(id: number): void {
    this.route.navigate(["slotinfo/" + id]);
  }
  userslot(id:string):boolean{
    //  alert(id);
    if (this.auth.isAuthenticated()) {
      var userid = localStorage.getItem(ACCESS_ID);
      return id == userid
    }
    else {
      return false;
    }
  }
}
