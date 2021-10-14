import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../_services/account.service';
import { ConfirmEmail } from '../../_models/confirmEmail';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-email',
  templateUrl: './email.component.html',
  styleUrls: ['./email.component.css']
})
export class EmailComponent implements OnInit {

  conformEmailData: ConfirmEmail;


  constructor(public accountService: AccountService, private router: Router, private route: ActivatedRoute, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {

      console.log(params);

      this.conformEmailData = {
        token: params.token,
        userId: params.userid
      }

    })
    
    this.confirmEmail(this.conformEmailData);
  }


  confirmEmail(data: ConfirmEmail){
 
    this.accountService.confirmEmail(data).subscribe(response => {
      console.log(response);
      this.toastr.success("Email confirmed!")
      this.router.navigateByUrl('/login');
    }, error => {
      this.toastr.error("Email confirmed error!")
      this.router.navigateByUrl('/login');
    });
}

}
