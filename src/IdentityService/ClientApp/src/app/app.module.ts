import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import {IdentityService} from "./services/identity.service";
import {BaseComponent} from "./ui/base/base.component";
import {RegisterComponent} from "./ui/components/register/register.component";
import {LoginComponent} from "./ui/components/login/login.component";

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    //
    BaseComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      {path: 'login', component: LoginComponent, pathMatch: 'full'},
      {path: 'register', component: RegisterComponent, pathMatch: 'full'},
      {path: '**', redirectTo: 'login'},
    ]),
  ],
  providers: [IdentityService],
  bootstrap: [AppComponent]
})
export class AppModule { }
