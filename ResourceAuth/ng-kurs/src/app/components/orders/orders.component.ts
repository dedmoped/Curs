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
  userwithmaxprice: any
  constructor(private bs :SlotstoreService) { }

  ngOnInit(): void {
    this.bs.getOrders().subscribe(res => { this.orders = res})
  }
  foo(slotid: string): string{
    var userid = localStorage.getItem(ACCESS_ID);
    var user_price;
    this.bs.getUserPrice(userid, slotid).subscribe(res => { user_price=res});
    var userprice = $.ajax({ async: false, url: "http://localhost/api/orders/uspri/" + userid + "/" + slotid, type: 'GET' }).responseText;
    console.log(userprice);
    console.log(user_price);
    return user_price;
  }

  getmaxprice(id: string){
    this.bs.getuseremail(id).subscribe(res => {
      (<HTMLInputElement>document.getElementById(id)).value = res["username"];
    },
      error => {
        alert("error")
      }
    );
  }
  RemoveOrder(id: number): void {
    this.bs.removeorder(id).subscribe(res => {

      this.ngOnInit();
    },
      error => {
        alert("error")
      }
    );
  }



}
