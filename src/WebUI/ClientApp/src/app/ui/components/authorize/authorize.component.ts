import {Component, OnInit} from '@angular/core';
import {OidcSecurityService} from "angular-auth-oidc-client";
import {Router} from "@angular/router";

@Component({
  selector: 'app-authorize',
  templateUrl: './authorize.component.html',
  styleUrls: ['./authorize.component.scss']
})
export class AuthorizeComponent implements OnInit {
  constructor(private oidcSecurityService: OidcSecurityService, private router: Router) {
  }

  ngOnInit(): void {
    this.oidcSecurityService.checkAuth().subscribe(({isAuthenticated, userData}) => {
      if (!isAuthenticated) {
        this.oidcSecurityService.authorize();
      } else {
        this.router.navigate(['/']);
      }
    });
  }

}
