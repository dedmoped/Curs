import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {

  constructor(private as:AuthService, private router: Router) { }

  ngOnInit(): void {

  }
  public get isLoggedIn():boolean{
    return this.as.isAuthenticated()
  }
  login(email:string,password:string){
    this.as.login(email,password).subscribe(res=>
      {

      },error=>{
        alert("Wrong login or password")
      })
  }
  logout(){
    this.as.logout() 
  }
  register(){
    this.router.navigate(["register"]);
  }
}
