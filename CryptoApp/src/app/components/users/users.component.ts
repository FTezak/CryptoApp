import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { UsersService } from '../../_services/users.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  constructor(private http: HttpClient, public accountService: AccountService, public usersService: UsersService) { }

  ngOnInit(): void {
    this.usersService.getUsers();
    this.getUser();
  }

  getUser(){
    console.log("Dohvacam usera");
    this.usersService.getUser(30).subscribe(response => {
      console.log("ODGOVOR --> " , response);
    }, error => {
      console.log(error);
    })
  }

}
