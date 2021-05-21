import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, OnInit, HostListener, ElementRef, QueryList, ViewChildren, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { from, Observable } from 'rxjs';
import { Slots } from 'src/app/models/slot';
import {SlotstoreService} from 'src/app/services/slotstore.service'
import * as $ from 'jquery';
import { element } from 'protractor';
import { FormControl, Validators } from "@angular/forms"
import { AuthService } from '../../services/auth.service';
import { error } from '@angular/compiler/src/util';
import { tap, filter,map } from 'rxjs/operators';
import { DataService } from '../../services/data.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
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
  loadbar: boolean = false;
  page: number = 1;
  filterText: string;
  data: any;
  nodata: boolean = false;
  is_Calback: boolean = false;
  myToday: any;
  today = new Date();
  lotTypes:any;
  innerWidth:any;
  type: number=0;
  status: number = 1;
  imgload: boolean = true;
  nowdate = new Date().toISOString().substring(0, 16);
  sort:boolean=false;
  //ctrl= new FormControl(null,Validators.required);
  constructor(private bs: SlotstoreService, private router: ActivatedRoute ,  private ds: DataService, private http: HttpClient, private route: Router, private auth: AuthService,public dialog: MatDialog) { }
  
  ngOnInit(): void {
    this.innerWidth=window.innerWidth;
    this.getcategories();
    this.myToday = new Date(this.today.getFullYear(), this.today.getMonth(), this.today.getDate(), 0, this.today.getMinutes() + 60, 0);
    setInterval(() => { this.myToday -= 100, this.convertTodate() }, 1000);
    console.log(this.router.snapshot.data.userposts);
    //this.bs.getCatalog().subscribe(res => { this.posts = res });
    this.ds.currentMessage.subscribe(message => this.filterText = message);
    
  }
  checkdate(db: string) {
    var time = (Date.parse(db) - new Date().getTime()) > 0;
    if (this.status == 1 && !time) {
      return true;
    }
    if (this.status == 2 && time) {
      return true;
    }
    return false;
  }
  chektime(db: string) {
    return (Date.parse(db) - new Date().getTime()) > 0;
  }

  convertTodate() {
    return this.today.toISOString().substring(0, 16);
  }
  getPosts() {
    this.is_Calback = true;
    this.page = this.page + 1;
    console.log(this.page);
    this.loadbar = true;
    this.bs.getCatalog(this.page,this.type,this.sort).subscribe((res) => this.onSuccess(res))   
  }
  loading: boolean = true
  onLoad() {
    this.loading = false;
  }
  ngAfterViewInit() {
 
  }
foo(num:number){
  this.page=1;
  this.type = num;
  this.loadbar = true;
  this.posts = null;
  this.bs.getCatalog(this.page, num, this.sort).subscribe((res) => { this.posts = res, this.loadbar=false});   
  }
  foo1(num: number) {
    this.page = 1;
    this.status = num;
    this.loadbar = true;
    this.posts = null;
    this.bs.getCatalog(this.page, this.type, this.sort, num).subscribe((res) => { this.posts = res, this.loadbar = false });
  }
getcategories(){
  this.bs.getTypes().subscribe(res=>{this.lotTypes=res,console.log(res)});
  }
  //load() {
  //  this.imgload = false;
  //}
  deleteslot(id: number) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent);
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.bs.deleteslot(id).subscribe(res => {
          console.log(id)
          // this.posts.pipe(map(pr => pr.filter(slot => slot.id != id)))
          this.posts=this.posts.filter(slot => slot.id != id);
        },
          error=>
          {
            console.log("")
        });
      }
    })
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
  onSuccess(res) {
    this.is_Calback = false;
    console.log(res);
    if (res != undefined && res.length != 0) {
      res.forEach(item => {
        this.posts.push(item);
      });
    }
    else {
      this.nodata = true;
    }
    this.loadbar = false;
  }

  //@HostListener("window:scroll", ["$event"])
  //onWindowScroll() {
  //  //In chrome and some browser scroll is given to body tag
  //  let pos = (document.documentElement.scrollTop || document.body.scrollTop) + document.documentElement.offsetHeight;
  //  let max = document.documentElement.scrollHeight;
  //  // pos/max will give you the distance between scroll bottom and and bottom of screen in percentage.
  //  if (max == pos) {
  //    this.page = this.page + 1;
  //    this.getPosts();
  //  }
  //  console.log(max)
  //  console.log(pos)
  //}

  @HostListener("window:scroll", [])
  onScroll(): void {
    if (!this.nodata) {
      if (this.bottomReached()) {
        var scrollHeight = Math.max(
          document.body.scrollHeight, document.documentElement.scrollHeight,
          document.body.offsetHeight, document.documentElement.offsetHeight,
          document.body.clientHeight, document.documentElement.clientHeight
        );

        if (scrollHeight - document.documentElement.offsetHeight - window.scrollY <= 100) {
          if (!this.is_Calback)
          this.getPosts();
        }
      }
    }
  }

  bottomReached(): boolean {
    return (window.innerHeight + window.scrollY) >= document.body.offsetHeight;
  }

@HostListener('window:resize',['$event'])
onResize(event){
this.innerWidth=window.innerWidth;
console.log(this.innerWidth);
}
}

