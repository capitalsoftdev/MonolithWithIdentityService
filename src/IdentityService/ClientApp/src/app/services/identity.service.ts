import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, catchError, Observable} from 'rxjs';
import {map} from 'rxjs/operators';

import {environment} from '../../environments/environment';
import {HttpHeaders} from '@angular/common/http';
import {AppConstant} from "../core/constants/app.constant";
import {AuthModel} from "../core/models/auth.model";

@Injectable({providedIn: 'root'})
export class IdentityService {
  url: string = "Identity";

  constructor(
    private router: Router,
    private http: HttpClient
  ) {
  }


  login(authModel: AuthModel): Observable<any> {
    return this.http.post(`${this.url}/Login`, authModel, {responseType: 'text'}).pipe(
      catchError((error, caught) => {
        console.log('login error', error);
        throw caught;
      }));
  }

  register(authModel: AuthModel): Observable<any> {
    return this.http.post(`${this.url}/Register`, authModel, {responseType: 'text'}).pipe(
      catchError((error, caught) => {
        console.log('register error', error);
        throw caught;
      }));
  }

}
