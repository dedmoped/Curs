import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { analyzeAndValidateNgModules } from '@angular/compiler';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, delay } from 'rxjs/operators';
import { Store_API_URL } from '../app-tokens';
import { OrdersComponent } from '../components/orders/orders.component';
import { Slots } from '../models/slot'
import { BehaviorSubject } from 'rxjs';
import { promise } from 'protractor';

@Injectable({
  providedIn: 'root'
})
export class SlotstoreService {
  getCurrentUserRating(sellerid: number) {
    return this.http.get(`${this.apiUrl}api/slots/getcurrentuserrate/` + sellerid)
  }
  setRate(sellerid: number, currentRate: number) {
    return this.http.post(`${this.apiUrl}api/slots/setrate/` + sellerid + `/` + currentRate, {});
    }

private baseApiUrl=`${this.apiUrl}api/`;
  constructor(private http: HttpClient,
    @Inject(Store_API_URL) 
    private apiUrl:string) { }
  lol: any;
  getCatalog(numb: number): Observable<Slots[]>{
    return this.http.get<Slots[]>(`${this.baseApiUrl}slots/lotList/` + numb).pipe(delay(1000));
  }

byeslot(slotid:number,newprice:number){
  return this.http.post(`${this.baseApiUrl}slots/byeslot/` + slotid + "/" + newprice, {});
}

getOrders():Observable<Slots[]>
{
  return this.http.get<Slots[]>(`${this.baseApiUrl}orders`)
  }
  getUserPrice(userid: string, slotid: string) {
    return this.http.get(`${this.baseApiUrl}orders/uspri/` + userid + "/" + slotid)
  }
getOrderById(id:number):Observable<Slots[]>
{
  return this.http.get<Slots[]>(`${this.baseApiUrl}orders/getslotbyid/`+id)
  }
  deleteslot(id: number){
    console.log(`${this.baseApiUrl}slots/deleteslot/` + id)
    return this.http.delete <string>(`${this.baseApiUrl}slots/deleteslot/` + id)
  }

  removeorder(id: number) {
    return this.http.delete(`${this.baseApiUrl}orders/deleteorder/` + id)
  }
  getuseremail(id: string) {
    return this.http.get(`${this.baseApiUrl}orders/getmaxprice/`+id)
  }
  getuserprice(slotid: string,userid:string) {
    return this.http.get(`${this.baseApiUrl}orders/uspri/` + userid + "/" + slotid);
  }
 addOrder(id:number):Observable<any>{
   return this.http.post(`${this.baseApiUrl}orders/add/`+id,{});
}
  addSlot(slot:Slots,file:File)
{
  let formdata= new FormData();
  formdata.append("pic",file);
  formdata.append("slotinfo",JSON.stringify(slot));
    return this.http.post(`${this.baseApiUrl}slots/addslot`,formdata);
}
updateSlot(slot:Slots,file:File)
{
  let formdata= new FormData();
  formdata.append("pic",file);
  formdata.append("slot",JSON.stringify(slot));
  return this.http.post(`${this.baseApiUrl}orders/update`,formdata);
}
getSlotRating(sellerid:string){
  return this.http.get(`${this.baseApiUrl}slots/rate/`+sellerid)
}
}
