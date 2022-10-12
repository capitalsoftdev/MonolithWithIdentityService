import {RouterModule, Routes} from "@angular/router";
import {HomeComponent} from "./ui/components/home/home.component";
import {AuthorizeComponent} from "./ui/components/authorize/authorize.component";

const appRoutes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'home', component: HomeComponent},
  {path: 'authorize', component: AuthorizeComponent},
];

export const routing = RouterModule.forRoot(appRoutes, {relativeLinkResolution: 'legacy'});
