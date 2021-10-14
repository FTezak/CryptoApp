import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent implements OnInit {

  url: any;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.url = navigation?.finalUrl;
   }

  ngOnInit(): void {
  }

}
