import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Slots } from '../models/slot';
import { SlotstoreService } from '../services/slotstore.service';

@Injectable()
export class UserPostsResolve implements Resolve<Slots[]> {
  constructor(private postService: SlotstoreService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<Slots[]> {
    return this.postService.getCatalog(1);
  }
} 
