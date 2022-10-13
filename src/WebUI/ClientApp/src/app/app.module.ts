import {NgModule, APP_INITIALIZER} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {AppComponent} from './app.component';
import {routing} from './app.routes';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {AuthModule, LogLevel, OidcSecurityService} from 'angular-auth-oidc-client';
import {HomeComponent} from "./ui/components/home/home.component";
import {TestService} from "./providers/services/test.service";
import {AppConstant} from "./core/constants/app.constant";


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
        authority: AppConstant.ReturnUrl,
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
  ],
  providers: [
    TestService,
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
