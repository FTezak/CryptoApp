import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { HomeComponent } from '../home/home.component';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { UsersService } from '../../_services/users.service';
import { map } from 'rxjs/operators';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;

  validationErrors: string[] = [];

  constructor(public accountService: AccountService, public usersService: UsersService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
    })
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : {notMatching: true}
    }
  }

  addUser(){
    console.log(this.registerForm.value);
    this.usersService.register(this.registerForm.value).subscribe(response => {
      this.toastr.success("User added!")
      map((response: User ) => {
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.accountService.setCurrentUser(user);
        }
      })
      this.router.navigateByUrl('/users');
      
    }, error => {
      this.validationErrors = error;
    })
  }

  register(){
 
      this.accountService.register(this.registerForm.value).subscribe(response => {
        console.log(response);
        this.toastr.success("Registration successful! Please confirm your email");
        this.router.navigateByUrl('/login');
      })

  }

  

}
