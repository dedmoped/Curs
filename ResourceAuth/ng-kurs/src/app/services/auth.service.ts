import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Inject, Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { Auth_API_URL } from '../app-tokens';
import { tap } from 'rxjs/operators';
import { Token } from 'src/app/models/token'

export const ACCESS_TOKEN_KEY = 'slotstore_access_token'
export const ACCESS_ID = 'slotstore_access_id'
export const ACCESS_NAME = 'slotstore_access_name'

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient,
    @Inject(Auth_API_URL) private apiUrl: string,
    private jwtHelper: JwtHelperService,
    private router: Router) { }



  login(email: string, password: string): Observable<Token> {
    return this.http.post<Token>(`${this.apiUrl}api/auth/login`, { "email":email, "password":password }).pipe(tap(to => {
      localStorage.setItem(ACCESS_TOKEN_KEY, to.access_token),
        localStorage.setItem(ACCESS_ID, to.userid);
      localStorage.setItem(ACCESS_NAME, to.username);
      this.router.navigate([""])
    }))
  }



  isAuthenticated(): boolean {
    var token = localStorage.getItem(ACCESS_TOKEN_KEY);
    return token && !this.jwtHelper.isTokenExpired(token)
  }

  logout(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(ACCESS_ID);
    localStorage.removeItem(ACCESS_NAME);
    this.router.navigate([""]);
  }
}
