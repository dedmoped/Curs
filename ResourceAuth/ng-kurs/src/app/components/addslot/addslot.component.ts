
import { Component, OnInit } from '@angular/core';
import { from, Subscription } from 'rxjs';
import { Slots } from 'src/app/models/slot';
import {SlotstoreService} from 'src/app/services/slotstore.service'
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-addslot',
  templateUrl: './addslot.component.html',
  styleUrls: ['./addslot.component.scss']
})
export class AddslotComponent implements OnInit {

  constructor(private bs: SlotstoreService, private auth: AuthService, private router: Router) 
  { 
  }
  slots: Slots[]
  changes:Slots = new Slots();
  private subscription:Subscription;

  files: any = [];
  imgUrl:any=[];
  file:any;
  
  ngOnInit(): void {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(["auth"])
    }
  }
  checkphoto():boolean{
   return this.files.length
  }
  
  uploadFile(event) {
    for (let index = 0; index < event.length; index++) {
      const element = event[index];
      this.files[0]=element
      var reader= new FileReader();
      reader.readAsDataURL(element);
      reader.onload=(_event)=>{
      this.imgUrl[0]=reader.result;
     }
      
  }
}

  addslot(price: number, description: string, seller: string): void {
    if (price && (description && this.files[0])) {

      this.changes.price = price.toString();
      this.changes.description = description;
      this.changes.seller = seller;
      this.bs.addSlot(this.changes, this.files[0]).subscribe();
    }
    else {
      alert("Проверьте поля и картинку")
    }
  }
}
