import { Component, OnInit } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Label } from 'ng2-charts';
import { Statistic } from 'src/app/models/Statistic';
import { AccountService } from 'src/app/services/account.service';
@Component({
  selector: 'app-ratingstatistic',
  templateUrl: './ratingstatistic.component.html',
  styleUrls: ['./ratingstatistic.component.scss']
})
export class RatingstatisticComponent implements OnInit {

  constructor(private ac : AccountService) { }
  stats: any[] = [];
  label = 'Рейтинг продавцов';
  isLoadingResults = true;
  barChartOptions: ChartOptions = {
    responsive: true,
    scales: {
      yAxes: [{
        ticks: {
          stepSize:1,
          max:5,
          min:0
        }
      }]
    }
  };
  barChartLabels: Label[] = [];
  barChartType: ChartType = 'bar';
  barChartLegend = true;
  barChartPlugins = [];
  barChartData: ChartDataSets[] = [{ data: [], backgroundColor: [], label: this.label}];
  ngOnInit(): void {
    this.getStatistic();
  }
  getStatistic() {
    this.barChartData = [{ data: [], backgroundColor: [], label: this.label }];
    this.barChartLabels = [];
    this.ac.getStatistic()
      .subscribe((res: any) => {
        this.stats = res;
        console.log(this.stats);
        const chartdata: number[] = [];
        const chartcolor: string[] = [];
        this.stats.forEach((stat) => {
          this.barChartLabels.push(stat.email);
          chartdata.push(stat.statistic);
          if (this.label === 'Рейтинг продавцов') {
            chartcolor.push('rgba(0, 255, 0, 0.5)');
          } 
        });
        this.barChartData = [{ data: chartdata, backgroundColor: chartcolor, label: this.label }];
        this.isLoadingResults = false;
      }, err => {
        console.log(err);
        this.isLoadingResults = false;
      });
  }
  changeStatus() {
    this.isLoadingResults = true;
    this.getStatistic();
  }
}
