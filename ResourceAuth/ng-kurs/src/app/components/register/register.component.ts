import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Inject, Injectable } from '@angular/core';
import { Store_API_URL } from 'src/app/app-tokens';
import { FormGroup, FormControl, Validators,FormBuilder } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import {ToastrService} from 'ngx-toastr';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})

export class RegisterComponent implements OnInit {

  constructor(private route: Router, private toastr : ToastrService, @Inject(Store_API_URL) private apiUrl: string, private formBuilder: FormBuilder, private au: AuthService) { }
  Roles: any = ['admin', 'author', 'user'];
  registerForm: FormGroup;
  submitted = false;
  registr: boolean = false;

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required]],
      phone: ['', [Validators.required,Validators.pattern("[0-9]{12}")]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, {
      validator: MustMatch('password', 'confirmPassword')
    });

  }

  login(email: string, password: string) {

    alert($.ajax({async:false,url: `${this.apiUrl}api/auth/register/`+email+"/"+password,type:'POST'}).responseText);
    this.route.navigate(["auth"]);
  }

  get f() { return this.registerForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }
    this.registr = true;
    this.au.register(this.registerForm.controls['email'].value, this.registerForm.controls['password'].value, this.registerForm.controls['phone'].value, this.registerForm.controls['name'].value).subscribe(res => { this.toastr.success(res['message']), this.registr = false; console.log(res) }, err => { this.toastr.error(err.error), this.registr = false; console.log(err) });
    //this.login(this.registerForm.controls['email'].value, this.registerForm.controls['password'].value);
  }
  showtoastr() {
    this.toastr.success("hello", "dsdfd");
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
