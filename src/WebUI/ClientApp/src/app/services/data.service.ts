import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';

import {Observable} from 'rxjs';
import {OidcSecurityService} from 'angular-auth-oidc-client';
import {OrdersByGrid} from "../models/OrderByGrid";

@Injectable()
export class DataService {

  private actionUrl: string;
  private actionUrlTest: string;
  private headers: HttpHeaders = new HttpHeaders();
  private token = "";

  constructor(private http: HttpClient, private securityService: OidcSecurityService) {
    this.actionUrl = `api/Binance`;
    this.actionUrlTest = `api/Test`;

    //this.securityService.getAccessToken();
    //   .subscribe((data:any) => {
    //   console.log("AAAAAAAAAAAAAAAAAA", data);
    //   this.token = data;
    // });
  }

  private setHeaders(): any {
    this.headers = new HttpHeaders();
    this.headers = this.headers.set('Content-Type', 'application/json');
    this.headers = this.headers.set('Accept', 'application/json');

    const token = this.securityService.getAccessToken();
    if (token !== '') {
      const tokenValue = 'Bearer ' + token;
      this.headers = this.headers.append('Authorization', tokenValue);
    }
  }

  public GetData = (): Observable<any> => {
    this.setHeaders();
    return this.http.get(this.actionUrlTest + "/GetData", { headers: this.headers });
  }
  public GetData2 = (): Observable<any> => {
    this.setHeaders();
    return this.http.get(this.actionUrlTest + "/GetData2", { headers: this.headers });
  }


  public saveKeys = (apiKey:string,secretKey:string): Observable<any> => {
    this.setHeaders();
    return this.http.post(this.actionUrl + "/SaveUserKeys", {"apiKey":apiKey,"secretKey":secretKey}, {headers: this.headers});
  }
  public ordersByGrid = (cmd:OrdersByGrid): Observable<any> => {
    this.setHeaders();
    return this.http.post(this.actionUrl + "/OrdersByGrid", cmd, {headers: this.headers});
  }


}
