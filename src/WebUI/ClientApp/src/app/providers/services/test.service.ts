import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';

import {Observable} from 'rxjs';
import {OidcSecurityService} from 'angular-auth-oidc-client';

@Injectable()
export class TestService {

  private readonly url: string = `api/Test`;
  private headers: HttpHeaders = new HttpHeaders();

  constructor(private http: HttpClient, private securityService: OidcSecurityService) {
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
    return this.http.get(this.url + "/GetData", { headers: this.headers });
  }



}
