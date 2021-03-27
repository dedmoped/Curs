import { Component } from '@angular/core';
import {AuthService} from './services/auth.service'
import { HomeComponent } from './components/home/home.component';
import { DataService } from './services/data.service';
export const ACCESS_NAME = 'slotstore_access_name'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'app';
  searchText: any;
  public get isLoggedIn():boolean{
    return this.as.isAuthenticated()
  }
  constructor(private as: AuthService, private ds: DataService) {

  }
  onChange() {
    console.log(this.searchText)
    this.ds.changeMessage(this.searchText);
  }
  login(email:string,password:string){
    this.as.login(email,password).subscribe(res=>
      {
      },error=>{
        alert("Wrong login or password")
      })
  }
  logout() {
    this.as.logout() 
  }
  getEmail(): string {
    return localStorage.getItem(ACCESS_NAME);
  }
}
