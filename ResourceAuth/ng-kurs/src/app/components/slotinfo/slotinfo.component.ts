import { Component, OnInit } from '@angular/core';
import { Slots } from 'src/app/models/slot';
import { SlotstoreService } from 'src/app/services/slotstore.service'
import { AuthService } from 'src/app/services/auth.service'
import {ActivatedRoute} from '@angular/router'
import { Subscription, Observable } from 'rxjs';
import { Inject, Injectable } from '@angular/core';
import { Store_API_URL } from 'src/app/app-tokens';
import {FormControl,Validators} from "@angular/forms"
import { map } from 'jquery';
@Component({
  selector: 'app-slotinfo',
  templateUrl: './slotinfo.component.html',
  styleUrls: ['./slotinfo.component.scss']
})
export class SlotinfoComponent implements OnInit {

  constructor(private bs: SlotstoreService, private auth: AuthService, private route: ActivatedRoute, @Inject(Store_API_URL) private apiUrl: string) 
  { 
this.subscription=route.params.subscribe(params=>this.id=params['id']);
  }
  slots: Slots[] = [];
  firstslot: Slots[];
  id: number;
  isauth: boolean;
  ratingnow:string;
  private subscription:Subscription;
  rate= new FormControl(null,Validators.required);
  ngOnInit(): void {
   this.bs.getOrderById(this.id).subscribe(res => { this.slots=res })
    this.isauth=this.auth.isAuthenticated();
  }

  takeslot(newprice: number) {

    console.log(this.slots);
    this.bs.byeslot(this.slots[0].id, newprice).subscribe()
  }
  lol() {
    alert("kek")
  }
  changerate(){
    var thisuser = localStorage.getItem('slotstore_access_id');
    var number = $.ajax({ async: false, url: `${this.apiUrl}api/slots/setrate/` + thisuser + "/" + this.slots[0].user_id + "/" + this.rate.value, type: 'POST' }).responseText;
    this.rate.setValue(Math.round(parseInt(number)))
    this.ratingnow = $.ajax({ async: false, url: `${this.apiUrl}api/slots/rate/` + this.slots[0].cost, type: 'GET' }).responseText;
  }
}
