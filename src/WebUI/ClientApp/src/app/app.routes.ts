import { HomeComponent } from './home/home.component';
import {UserKeysComponent} from "./user-keys/user-keys.component";
import {OrdersByGridComponent} from "./orders-by-grid/orders-by-grid.component";
import {RouterModule, Routes} from "@angular/router";

const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'home', component: HomeComponent },
    { path: 'user-keys', component: UserKeysComponent },
    { path: 'ordersByGrid', component: OrdersByGridComponent },
];

export const routing = RouterModule.forRoot(appRoutes, { relativeLinkResolution: 'legacy' });
