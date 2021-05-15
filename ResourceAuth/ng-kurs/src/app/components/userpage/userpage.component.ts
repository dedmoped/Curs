import { Component, OnInit } from '@angular/core';
import {FormGroup, FormBuilder, FormControl, FormGroupDirective, NgForm, Validators} from '@angular/forms';
import { FindValueSubscriber } from 'rxjs/internal/operators/find';
import { account } from 'src/app/models/account';
import { AccountService } from 'src/app/services/account.service';
@Component({
  selector: 'app-userpage',
  templateUrl: './userpage.component.html',
  styleUrls: ['./userpage.component.scss']
})
export class UserpageComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private ac: AccountService) { }
  updateAccount: account;
  account:account;
  files: any = [];
  imgUrl:any=[];
  userpageValidation: FormGroup;
  submitted=false;
  showForm=false;
  ngOnInit(): void {
    this.ac.getAccount().subscribe(res => { this.account = res, console.log(this.account)}, err => console.log('errorgetaccoutinfo'));
    this.userpageValidation = this.formBuilder.group({
      Title: ['', [Validators.required, Validators.maxLength(20)]],
      Name:['',[Validators.required]],
      Email:['',[Validators.required,Validators.email]],
      Phone:['',[Validators.required,Validators.pattern("[0-9]{12}")]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, {
      validator: MustMatch('password', 'confirmPassword')
    });
  }
  checkphoto():boolean{
    return this.files.length
   }
   shoForm(){
     this.showForm=true;
   }
   hideForm(){
    this.showForm=false;
  }
   get f() { return this.userpageValidation.controls; }

   uploadFile(event) {
     for (let index = 0; index < event.length; index++) {
       const element = event[index];
       this.files[0]=element;
       var reader= new FileReader();
       reader.readAsDataURL(element);
       reader.onload=(_event)=>{
       this.imgUrl[0]=reader.result;
      }
   }
 
  }
  removephoto(){
    this.files.length=0;
  }
  onSubmit(){
    this.submitted=true;
    if (this.userpageValidation.invalid) {
      return;
    }

    this.updateAccount.Description = this.userpageValidation.controls['Title'].value;
    this.updateAccount.Name = this.userpageValidation.controls['Name'].value;
    this.updateAccount.Mobile = this.userpageValidation.controls['Phone'].value;
    this.updateAccount.Email = this.userpageValidation.controls['Email'].value;
    this.updateAccount.Email = this.userpageValidation.controls['password'].value;

    this.ac.updateAccount(this.updateAccount, this.files[0]).subscribe();
  }
}
export function MustMatch(controlName: string, matchingControlName: string) {
  return (formGroup: FormGroup) => {
    const control = formGroup.controls[controlName];
    const matchingControl = formGroup.controls[matchingControlName];

    if (matchingControl.errors && !matchingControl.errors.mustMatch) {
      // return if another validator has already found an error on the matchingControl
      return;
    }

    // set error on matchingControl if validation fails
    if (control.value !== matchingControl.value) {
      matchingControl.setErrors({ mustMatch: true });
    } else {
      matchingControl.setErrors(null);
    }
  }
}

