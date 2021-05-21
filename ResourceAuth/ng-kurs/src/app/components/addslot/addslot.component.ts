
import { Component, OnInit } from '@angular/core';
import { from, Subscription } from 'rxjs';
import { Slots } from 'src/app/models/slot';
import {SlotstoreService} from 'src/app/services/slotstore.service'
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {FormGroup, FormBuilder, FormControl, FormGroupDirective, NgForm, Validators} from '@angular/forms';
@Component({
  selector: 'app-addslot',
  templateUrl: './addslot.component.html',
  styleUrls: ['./addslot.component.scss']
})
export class AddslotComponent implements OnInit {

  constructor(private bs: SlotstoreService, private toastr: ToastrService,private formBuilder: FormBuilder, private auth: AuthService, private router: Router) 
  { 
  }
  slots: Slots[]
  types: any;
  changes:Slots = new Slots();
  private subscription:Subscription;
  hasphoto:boolean=true;
  files: any = [];
  imgUrl:any=[];
  file: any;
  spinerlot: boolean = false;
  mindate = new Date().toISOString().substring(0, 16);
  today = new Date();
  myToday: any;
  slotValidation: FormGroup;
  ngOnInit(): void {
    this.myToday = new Date(this.today.getFullYear(), this.today.getMonth(), this.today.getDate(), 0, this.today.getMinutes()+60, 0).toISOString().substring(0, 16);
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(["auth"])
    }
    this.slotValidation = this.formBuilder.group({
      Title: ['', [Validators.required, Validators.maxLength(20)]],
      Cost:['',[Validators.required]],
      Type: ['', Validators.required],
      StartDate: [this.mindate, Validators.required],
      EndDate: [this.myToday,Validators.required],
    });
    this.bs.getTypes().subscribe(x => { this.types = x }, err => alert("Ошибка получения типов"));
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
     this.hasphoto=false;
  }
}
foods = [
  {value: 'steak-0', viewValue: 'Steak'},
  {value: 'pizza-1', viewValue: 'Pizza'},
  {value: 'tacos-2', viewValue: 'Tacos'}
];
removephoto(){
  this.files.length=0;
  this.hasphoto=true;
}

  addslot(price: number, description: string, seller: string): void {
    if (price && (description && this.files[0])) {

      this.changes.cost = price.toString();
      this.changes.description = description;
      this.changes.seller = seller;
      this.changes.title=this.slotValidation.controls['Title'].value;
      this.changes.endDate=this.slotValidation.controls['EndDate'].value;
      this.changes.startDate=this.slotValidation.controls['StartDate'].value;
      this.changes.type_id = this.slotValidation.controls['Type'].value;
      console.log(this.changes);
      this.spinerlot = true;
      this.bs.addSlot(this.changes, this.files[0]).subscribe(res => { this.spinerlot = false, this.toastr.success() }, err => { this.spinerlot = false, console.log(err) });
    }
    else {
      alert("Проверьте поля и картинку")
    }
  }
  tiles= [
    {text: 'One', cols: 3, rows: 1, color: 'lightblue'},
    {text: 'Two', cols: 1, rows: 3, color: 'lghtgreen'},
    {text: 'One', cols: 3, rows: 1, color: 'lightblue'},
    {text: 'One', cols: 3, rows: 1, color: 'lightblue'},
  ];
}
