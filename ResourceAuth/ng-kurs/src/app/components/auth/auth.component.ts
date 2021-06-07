import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {

  constructor(private as: AuthService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {

  }
  public get isLoggedIn():boolean{
    return this.as.isAuthenticated()
  }
  login(email:string,password:string){
    this.as.login(email,password).subscribe(res=>
      {
console.log(res);
    }, error => {
        this.toastr.error("Проверьте введенные данные и подтвердите почту");
      })
  }
  logout(){
    this.as.logout() 
  }
  register(){
    this.router.navigate(["register"]);
  }
}
