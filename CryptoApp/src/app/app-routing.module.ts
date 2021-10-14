import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './components/admin/admin.component';
import { ChangePasswordComponent } from './components/changePassword/change-password.component';
import { CryptoFavoriteComponent } from './components/crypto/crypto-favorite/crypto-favorite.component';
import { CryptoInfoComponent } from './components/crypto/crypto-info/crypto-info.component';
import { CryptoWalletComponent } from './components/crypto/crypto-wallet/crypto-wallet.component';
import { EmailComponent } from './components/emailConfirm/email.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { ServerErrorComponent } from './components/errors/server-error/server-error.component';
import { TestErrorsComponent } from './components/errors/test-errors.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { NewsletterComponent } from './components/newsletter/newsletter.component';
import { RegisterComponent } from './components/register/register.component';
import { UsersComponent } from './components/users/users.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { UsersService } from './_services/users.service';

const routes: Routes = [
  
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'users', component: UsersComponent},
      {path: 'admin', component: AdminComponent, canActivate: [AdminGuard]}
    ]
  },
  {path: 'register', component: RegisterComponent},
  {path: 'login', component: LoginComponent},
  {path: 'confirm-email', component: EmailComponent},
  {path: 'change-password', component: ChangePasswordComponent},
  {path: 'errors', component: TestErrorsComponent},
  {path: 'crypto/:crypto', component: CryptoInfoComponent},
  {path: 'fav-crypto', component: CryptoFavoriteComponent},
  {path: 'wallet', component: CryptoWalletComponent},
  {path: 'newsletter', component: NewsletterComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
