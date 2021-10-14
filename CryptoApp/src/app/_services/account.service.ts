import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { ReplaySubject } from 'rxjs';
import { ConfirmEmail } from '../_models/confirmEmail';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;

  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  users: any;
  model: any = {};


  constructor(private http: HttpClient){}

  login(model: any){
    return this.http.post(this.baseUrl + 'Account/login', model).pipe(
      map((response: User ) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model: any){
    return this.http.post(this.baseUrl + 'Account/register', model).pipe(
      map((response: User ) => {
        const user = response;
        //if(user){
          //this.setCurrentUser(user);
        //}
      })
    )
  }

  changePassword(model: any){
    console.log(model);
    
    return this.http.post(this.baseUrl + 'Account/changePassword', model).pipe(
      map((response: any ) => {
        console.log(response);
      })
    )
  }

  confirmEmail(data: ConfirmEmail){
    return this.http.post(this.baseUrl + 'Account/confirmEmail', data).pipe(
      map((response: any ) => {
        console.log(response);
      })
    )
  }

  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  getDecodedToken(token){
    return JSON.parse(atob(token.split('.')[1]));
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

}
