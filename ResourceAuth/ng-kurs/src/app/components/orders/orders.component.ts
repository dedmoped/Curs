import { Component, OnInit } from '@angular/core';
import { Slots } from 'src/app/models/slot';
import { SlotstoreService } from 'src/app/services/slotstore.service';
import { error } from 'jquery';
import { DataService } from '../../services/data.service';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../../services/auth.service';
export const ACCESS_ID = 'slotstore_access_id'
export const ACCESS_NAME = 'slotstore_access_name'

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  orders: Slots[]
  filterText: string;
  userPrice: any;
  nowdate = new Date().toISOString().substring(0, 16);
  userwithmaxprice: any
  constructor(private bs: SlotstoreService, private ds: DataService, public dialog: MatDialog, private auth: AuthService) { }

carouselOptions = {
    margin: 25,
    nav: true,
    center: true,
    navText: ["<div class='nav-btn prev-slide'></div>", "<div class='nav-btn next-slide'></div>"],
    responsiveClass: true,
    items: 1
  }
  ngOnInit(): void {
    this.ds.currentMessage.subscribe(message => this.filterText = message);
    this.bs.getOrders().subscribe(res => { this.orders = res["orders"], this.userPrice = res["userprice"], console.log(res["userprice"])})
  }
  foo(){
    console.log(this.userPrice)
    console.log("data");
  }

  isyours(email: string) {
    return localStorage.getItem(ACCESS_NAME) == email;
  }
  getByLots() {
    this.bs.getOrders().subscribe(res => { this.orders = res["orders"], this.userPrice = res["userprice"], console.log(res["userprice"]) })
  }

  getCreatedLots() {
    this.bs.getYourLots().subscribe(res => { this.orders = res["orders"], this.userPrice = res["userprice"], console.log(res["userprice"]) })
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
  checkdate(db: string) {
    return (Date.parse(db) - new Date().getTime()) > 0;
  }
  deleteslot(id: number) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent);
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.bs.deleteslot(id).subscribe(res => {
          console.log(id)
          // this.posts.pipe(map(pr => pr.filter(slot => slot.id != id)))
          this.orders = this.orders.filter(slot => slot.id != id);
        },
          error => {
            console.log("")
          });
      }
    })
  }
  userslot(id: string): boolean {
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
