import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  changePasswordForm: FormGroup;

  validationErrors: string[] = [];

  constructor(public accountService: AccountService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.changePasswordForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      oldPassword: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
    })
    this.changePasswordForm.controls.password.valueChanges.subscribe(() => {
      this.changePasswordForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : {notMatching: true}
    }
  }

  changePassword(){
 
    this.accountService.changePassword(this.changePasswordForm.value).subscribe(response => {
      console.log(response);
      this.toastr.success("Password changed!")
      this.router.navigateByUrl('/login');
    }, error => {
      this.toastr.error("Change password error!")
    })

}

}
