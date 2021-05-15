import { Component, OnInit, OnDestroy, Input  } from '@angular/core';
import { Subscription, interval } from 'rxjs';

@Component({
  selector: 'app-count-down',
  templateUrl: './count-down.component.html',
  styleUrls: ['./count-down.component.scss']
})
export class CountDownComponent implements OnInit, OnDestroy {

  private subscription: Subscription;

  @Input() dDay: any;
  @Input() dDayEnd: any;
  public dateNow = new Date();
  milliSecondsInASecond = 1000;
  hoursInADay = 24;
  minutesInAnHour = 60;
  SecondsInAMinute = 60;

  public timeDifference;
  public secondsToDday;
  public minutesToDday;
  public hoursToDday;
  public daysToDday;

  private getTimeDifference() {
    this.timeDifference = Date.parse(this.dDayEnd) - new Date().getTime();
    if (this.timeDifference <= 0)
      this.timeDifference = 0;
    this.allocateTimeUnits(this.timeDifference);
  }
  private allocateTimeUnits(timeDifference) {
    this.secondsToDday = Math.floor((timeDifference) / (this.milliSecondsInASecond) % this.SecondsInAMinute);
    this.minutesToDday = Math.floor((timeDifference) / (this.milliSecondsInASecond * this.minutesInAnHour) % this.SecondsInAMinute);
    this.hoursToDday = Math.floor((timeDifference) / (this.milliSecondsInASecond * this.minutesInAnHour * this.SecondsInAMinute) % this.hoursInADay);
    this.daysToDday = Math.floor((timeDifference) / (this.milliSecondsInASecond * this.minutesInAnHour * this.SecondsInAMinute * this.hoursInADay));
  }
  constructor() { }

  ngOnInit(): void {
    this.subscription = interval(1000)
      .subscribe(x => { this.getTimeDifference(); });
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
