import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, OnInit } from '@angular/core';
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
export const ACCESS_ID = 'slotstore_access_id'
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit,AfterViewInit {
   Slot:Slots[]=[]
  posts: Observable<Slots[]>;
  userRate: [];
  filterText: string;
  data: any;
  //ctrl= new FormControl(null,Validators.required);
  constructor(private bs: SlotstoreService, private router: ActivatedRoute ,  private ds: DataService, private http: HttpClient, private route: Router, private auth: AuthService) { }
   
  ngOnInit(): void {
    this.posts = this.router.snapshot.data.userposts;
    //this.bs.getCatalog().subscribe(res => { this.posts = res });
    this.ds.currentMessage.subscribe(message => this.filterText = message);
  }
  
  ngAfterViewInit(){
 
     
  }
  deleteslot(id: number) {
    this.bs.deleteslot(id).subscribe(res => {
      console.log(res)
      this.posts = this.posts.pipe(map(pr => pr.filter(slot => slot.id != id)))
    },
      error=>
      {
        console.log("")
    }
      );
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
}
