import {OidcSecurityService} from 'angular-auth-oidc-client';
import {Component, OnInit} from '@angular/core';
import {Observable, of} from 'rxjs';
import {environment} from "../environments/environment";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {
  userData = {};
  isAuthenticated = false;


  constructor(private oidcSecurityService: OidcSecurityService) {
  }

  ngOnInit(): void {
    //this.userData$ = this.oidcSecurityService.userData$;
    this.oidcSecurityService.checkAuth().subscribe(({isAuthenticated, userData}) => {
      this.isAuthenticated = isAuthenticated;
      this.userData = userData;
      console.log(this.isAuthenticated, userData)

      if (!this.isAuthenticated) {
        this.oidcSecurityService.authorize();
      }
    });
  }


  login(): void {
    this.oidcSecurityService.authorize();
  }

  logout(): void {
    localStorage.setItem("isOnline", "false");
    this.oidcSecurityService.logoffLocal();
    this.oidcSecurityService.logoff();
  }

}
