import { Component, OnInit } from '@angular/core';
import { Slots } from 'src/app/models/slot';
import { SlotstoreService } from 'src/app/services/slotstore.service';
import { error } from 'jquery';
export const ACCESS_ID = 'slotstore_access_id'

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  orders: Slots[]
  userPrice: any;
  userwithmaxprice: any
  constructor(private bs :SlotstoreService) { }

  ngOnInit(): void {
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
    console.log(ord);
    this.bs.removeorder(ord.id).subscribe(res => {
      this.orders.splice(this.orders.indexOf(ord), 1)
    },
      error => {
        alert("REMOVE ERROR")
      }
    );
  }



}
