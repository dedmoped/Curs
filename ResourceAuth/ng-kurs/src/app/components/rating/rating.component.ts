import { Component, Input, OnInit } from '@angular/core';
import { SlotstoreService } from 'src/app/services/slotstore.service';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.scss']
})
export class RatingComponent implements OnInit {

  @Input() sellerid: any;
  currentRate: any;
  userRating: any;
  current: any;
  @Input() readonl: boolean;
  constructor(private ds:SlotstoreService ) { }
  ngOnInit(): void {
    console.log("ngOnInit")
    this.ds.getSlotRating(this.sellerid).subscribe(res => { this.currentRate = res, this.current = res }, err => alert("error rating"));
    this.ds.getCurrentUserRating(this.sellerid).subscribe(res => { this.userRating = res }, err => alert("error"));
  }
  changerate() {
    this.ds.setRate(this.sellerid, this.currentRate).toPromise().then(
       (data)=> {
        this.userRating = data;
        this.ds.getSlotRating(this.sellerid).toPromise().then((info) => { this.currentRate = info; this.current = info });
          });
  }

}
