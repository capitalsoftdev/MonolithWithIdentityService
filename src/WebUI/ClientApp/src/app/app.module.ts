import {NgModule, APP_INITIALIZER} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {AppComponent} from './app.component';
import {routing} from './app.routes';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {HomeComponent} from './home/home.component';
import {AuthModule, LogLevel, OidcSecurityService} from 'angular-auth-oidc-client';
import {DataService} from "./services/data.service";
import {UserKeysComponent} from './user-keys/user-keys.component';
import {OrdersByGridComponent} from './orders-by-grid/orders-by-grid.component';


// export function initialize(initializeService: InitializeService) {
//   return (): Promise<any> => {
//     return initializeService.Init();
//   }
// }


@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    routing,
    HttpClientModule,
    AuthModule.forRoot({
      config: {
        authority: 'https://localhost:6001', // 'https://test10.capitalsoft.am', //debug 'https://localhost:6001'
        redirectUrl: window.location.origin,
        postLogoutRedirectUri: window.location.origin,
        clientId: 'angularclient',
        scope: 'openid profile email dataEventRecords offline_access',
        responseType: 'code',
        silentRenew: true,
        autoUserInfo:true,
        ignoreNonceAfterRefresh: true,
        renewTimeBeforeTokenExpiresInSeconds: 10,
        useRefreshToken: true,
        logLevel: LogLevel.Debug,

      },
    }),
  ],
  declarations: [
    AppComponent,
    HomeComponent,
    UserKeysComponent,
    OrdersByGridComponent
  ],
  providers: [
    DataService,
    OidcSecurityService
    // InitializeService,
    // {provide: APP_INITIALIZER, useFactory: initialize, deps: [InitializeService], multi: true}
  ],
  bootstrap: [AppComponent],
})

export class AppModule {
  constructor() {
  }
}
