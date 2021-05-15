import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Auth_API_URL } from '../app-tokens';
import { account } from '../models/account';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient,
    @Inject(Auth_API_URL) private apiUrl: string) { }
  private baseApiUrl = `${this.apiUrl}api/`;
  getAccount(): Observable<account>{
      return this.http.get<account>(`${this.baseApiUrl}accounts/Account`)
  }
  updateAccount(acc: account, file: File) {
    acco :account;
    let formdata = new FormData();
    formdata.append("pic", file);
    formdata.append("accounts", JSON.stringify(acc));
    return this.http.post(`${this.baseApiUrl}accounts/Account`, formdata);
    }
}
