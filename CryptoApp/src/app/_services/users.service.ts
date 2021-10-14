import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';



@Injectable({
  providedIn: 'root'
})
export class UsersService {

  

  baseUrl = environment.apiUrl;

  users: any;

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  getUsers(){

      this.http.get(this.baseUrl + 'Users').subscribe(response => {
      this.users = response;
    }, error => {
      console.log(error);
    })
  }

  removeUser(user: any){
    this.http.delete(this.baseUrl + 'Users/' + user, ).subscribe(response => {
      console.log(response);
      this.getUsers();
    })
  }

  addUser(user: any){
    this.http.post(this.baseUrl + 'Account/register', user).subscribe(response => {
      console.log(response);
      this.getUsers();
    })    
  }

  register(model: any){
    return this.http.post(this.baseUrl + 'Account/register', model);
  }

  getUser(id: any){

    let params = new HttpParams();
    params = params.append('id', id);

    return this.http.get(this.baseUrl + 'Users/user', {params: params});
  }

}
