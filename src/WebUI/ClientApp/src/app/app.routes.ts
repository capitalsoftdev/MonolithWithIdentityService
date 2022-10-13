import {RouterModule, Routes} from "@angular/router";
import {HomeComponent} from "./ui/components/home/home.component";

const appRoutes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'home', component: HomeComponent},
];

export const routing = RouterModule.forRoot(appRoutes, {relativeLinkResolution: 'legacy'});
