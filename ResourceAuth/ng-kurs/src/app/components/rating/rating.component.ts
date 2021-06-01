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
  @Input() order: boolean = false;
  spinerrating: boolean = true;
  currentRate: any;
  userRating: any=0;
  current: any;
  @Input() readonl: boolean;
  constructor(private ds:SlotstoreService ) { }
  ngOnInit(): void {
    console.log("ngOnInit")
    this.ds.getSlotRating(this.sellerid).subscribe(res => { this.currentRate = res, this.spinerrating = false }, err => { this.spinerrating = false });
    this.ds.getCurrentUserRating(this.sellerid).subscribe(res => { this.userRating = res, this.spinerrating = false }, err => {this.spinerrating = false });
  }
  changerate() {
    this.userRating = 0;
    this.ds.setRate(this.sellerid, this.currentRate).toPromise().then(
       (data)=> {
        this.userRating = data;
        this.ds.getSlotRating(this.sellerid).toPromise().then((info) => { this.spinerrating = false; this.currentRate = info;});
          });
  }

}
