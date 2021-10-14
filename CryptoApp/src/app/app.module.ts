import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NavComponent } from './components/nav/nav.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { ToastrModule } from 'ngx-toastr';
import { UsersComponent } from './components/users/users.component';
import { TestErrorsComponent } from './components/errors/test-errors.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { ServerErrorComponent } from './components/errors/server-error/server-error.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { AdminComponent } from './components/admin/admin.component';
import { HasRoleDirective } from './_directives/has-role.directive';
import { UserManagementComponent } from './components/admin/user-management/user-management.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { RolesModalComponent } from './components/modals/roles-modal/roles-modal.component';
import { LoginComponent } from './components/login/login.component';
import { EmailComponent } from './components/emailConfirm/email.component';
import { ChangePasswordComponent } from './components/changePassword/change-password.component';
import { CryptoInfoComponent } from './components/crypto/crypto-info/crypto-info.component';
import { DatePipe } from '@angular/common';
import { CryptoHomeComponent } from './components/crypto/crypto-home/crypto-home.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { CryptoFavoriteComponent } from './components/crypto/crypto-favorite/crypto-favorite.component';
import { CryptoWalletComponent } from './components/crypto/crypto-wallet/crypto-wallet.component';
import { BinaceKeysComponent } from './components/modals/binace-keys/binace-keys.component';
import { SearchComponent } from './components/nav/search/search.component';
import { WalletManagementComponent } from './components/crypto/wallet-management/wallet-management.component';
import { WalletModalComponent } from './components/modals/wallet-modal/wallet-modal.component';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { DecimalNumberDigitsDirective } from './components/modals/wallet-modal/decimal-number-digits.directive';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { NgxSpinnerModule } from "ngx-spinner";
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { NewsletterComponent } from './components/newsletter/newsletter.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    UsersComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent,
    TextInputComponent,
    AdminComponent,
    HasRoleDirective,
    UserManagementComponent,
    RolesModalComponent,
    LoginComponent,
    EmailComponent,
    ChangePasswordComponent,
    CryptoInfoComponent,
    CryptoHomeComponent,
    CryptoFavoriteComponent,
    CryptoWalletComponent,
    BinaceKeysComponent,
    SearchComponent,
    WalletManagementComponent,
    WalletModalComponent,
    DecimalNumberDigitsDirective,
    NewsletterComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    TooltipModule.forRoot(),
    PaginationModule.forRoot(),
    BsDropdownModule.forRoot(),
    TypeaheadModule.forRoot(),
    MatProgressSpinnerModule,
    NgxSpinnerModule,
    ButtonsModule.forRoot()
  ],
  exports: [
    PaginationModule,
    ModalModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
    HttpClientModule,
  DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
