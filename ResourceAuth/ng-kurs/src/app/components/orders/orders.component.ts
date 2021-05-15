import { Component, OnInit } from '@angular/core';
import { Slots } from 'src/app/models/slot';
import { SlotstoreService } from 'src/app/services/slotstore.service';
import { error } from 'jquery';
import { DataService } from '../../services/data.service';
export const ACCESS_ID = 'slotstore_access_id'

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  orders: Slots[]
  filterText: string;
  userPrice: any;
  userwithmaxprice: any
  constructor(private bs: SlotstoreService, private ds: DataService) { }

  ngOnInit(): void {
    this.ds.currentMessage.subscribe(message => this.filterText = message);
    this.bs.getOrders().subscribe(res => { this.orders = res["orders"], this.userPrice=res["userprice"]})
  }
  foo(){
    console.log(this.userPrice)
    console.log("data");
  }

  getmaxprice(id: string){
    this.bs.getuseremail(id).subscribe(res => {
      (<HTMLInputElement>document.getElementById(id)).value = res["username"];
    }
    );
  }
  RemoveOrder(ord: any): void {
    console.log(ord.id);
    this.bs.removeorder(ord.id).subscribe(res => {
      this.orders.splice(this.orders.indexOf(ord), 1)
    },
      error => {
        alert("REMOVE ERROR")
      }
    );
  }



}
