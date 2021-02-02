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
const routes: Routes = [
  {path:'',component:HomeComponent},
  {path:'orders',component:OrdersComponent},
  {path:'auth',component:AuthComponent},
  {path:'upload',component:UploadFileComponent},
  {path:'update/:id',component:UpdateComponent},
  {path:'addslot',component:AddslotComponent},
  {path:'register',component:RegisterComponent},
  { path: 'slotinfo/:id', component: SlotinfoComponent },
  { path: '**', component: ErroComponent },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
