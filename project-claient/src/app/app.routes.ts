import { Routes } from '@angular/router';
import { Gift } from './Components/gift/gift';
import { GiftList } from './Components/gift-list/gift-list';
import { ListModor } from './Components/list-modor/list-modor';
import { modor } from './Components/modor/modor';
import { Customer } from './Components/customer/customer';
import { ListCustomer } from './Components/list-customer/list-customer';
import { CustomerDetails } from './Components/customer-details/customer-details';
import { ReportWinnersComp } from './Components/report-winners-comp/report-winners-comp';
import { RegisterComp } from './Components/register-comp/register-comp';
import { LoginComp } from './Components/login-comp/login-comp';
export const routes: Routes = [
{path: 'home', component: GiftList},
{path: 'add-gift' , component:Gift },
{path: 'donor-list',component:ListModor},
{path:'add-donor',component:modor},
{path:'customer-list',component:ListCustomer},
{path:'add-customer',component:Customer},
{path:'customer-datails',component:CustomerDetails},
{path:'reportWinners',component:ReportWinnersComp},
{path:'register',component:RegisterComp},
{path:'login',component:LoginComp}
];
