import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {MatCardModule} from '@angular/material/card';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import {MatTableModule} from '@angular/material/table';
import {MatFormFieldModule} from '@angular/material/form-field';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './components/home/home.component';
import { OrdersComponent } from './components/orders/orders.component';
import { Auth_API_URL, Store_API_URL } from './app-tokens';
import { environment } from 'src/environments/environment';
import { JwtModule } from '@auth0/angular-jwt';
import { ACCESS_TOKEN_KEY } from './services/auth.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AuthComponent } from './components/auth/auth.component';
import { UploadFileComponent } from './components/upload-file/upload-file.component';
import { DragDropDirective } from './drag-drop.directive';
import { UpdateComponent } from './components/update/update.component';
import { AddslotComponent } from './components/addslot/addslot.component';
import { ChildComponent } from './components/child/child.component';
import {MatGridListModule} from '@angular/material/grid-list';
import {FormsModule,ReactiveFormsModule} from '@angular/forms';
import {MatIconModule} from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { RegisterComponent } from './components/register/register.component';
import { SlotinfoComponent } from './components/slotinfo/slotinfo.component';
import { ErroComponent } from './components/erro/erro.component';
import { RatingComponent } from './components/rating/rating.component';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { UserPostsResolve } from './resolvers/slot-posts.resolve';
import { SlimLoadingBarModule } from 'ng2-slim-loading-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NguiComponent } from './components/ngui/ngui.component';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';

export function tokenGetter(){
  return localStorage.getItem(ACCESS_TOKEN_KEY);
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    OrdersComponent,
    AuthComponent,
    UploadFileComponent,
    DragDropDirective,
    UpdateComponent,
    AddslotComponent,
    ChildComponent,
    RegisterComponent,
    SlotinfoComponent,
    ErroComponent,
    RatingComponent,
    NguiComponent,
    ConfirmationDialogComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatGridListModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatSelectModule,
    Ng2SearchPipeModule,
    MatProgressSpinnerModule,
    MatDialog,
    SlimLoadingBarModule.forRoot(),
    JwtModule.forRoot({
      config:{
        tokenGetter,
        allowedDomains:environment.tokenWhiteListedDomains
      }
    }),

    NgbModule
  ],
  providers: [{
    provide: Auth_API_URL,
    useValue: environment.authApi
  },
  {
    provide:Store_API_URL,
    useValue:environment.storeApi
    },
    HomeComponent,
    UserPostsResolve  
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
