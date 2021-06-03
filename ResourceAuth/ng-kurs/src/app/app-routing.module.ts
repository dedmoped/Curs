import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { from } from 'rxjs';
import { AuthComponent } from './components/auth/auth.component';
import {HomeComponent} from './components/home/home.component';
import {OrdersComponent} from './components/orders/orders.component';
import {UploadFileComponent} from './components/upload-file/upload-file.component'
import {UpdateComponent} from './components/update/update.component'
import {AddslotComponent} from './components/addslot/addslot.component'
import{RegisterComponent} from './components/register/register.component'
import { SlotinfoComponent } from './components/slotinfo/slotinfo.component'
import { ErroComponent } from './components/erro/erro.component';
import { UserPostsResolve } from './resolvers/slot-posts.resolve';
import { 
  AuthGuardService as AuthGuard 
} from './services/auth-guard.service';
import { UserpageComponent } from './components/userpage/userpage.component';
import { RatingstatisticComponent } from './components/ratingstatistic/ratingstatistic.component';
import { VerificationComponent } from './components/verification/verification.component';
const routes: Routes = [
  {
    path: '', component: HomeComponent, resolve: {
      userposts: UserPostsResolve
    } },
  {path:'orders',component:OrdersComponent,canActivate: [AuthGuard] },
  {path:'auth',component:AuthComponent},
  {path:'upload',component:UploadFileComponent,canActivate: [AuthGuard]},
  {path:'update/:id',component:UpdateComponent,canActivate: [AuthGuard]},
  {path:'addslot',component:AddslotComponent,canActivate: [AuthGuard]},
  {path:'register',component:RegisterComponent},
  { path: 'slotinfo/:id', component: SlotinfoComponent },
  { path: 'statistic', component: RatingstatisticComponent },
  {path:'userpage',component:UserpageComponent},
  {path:'verify',component:VerificationComponent},
  { path: '**', component: ErroComponent },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
