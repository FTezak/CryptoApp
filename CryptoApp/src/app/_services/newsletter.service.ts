import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NewsletterConfig } from '../_models/newsletterConfig';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsletterService {

  baseUrl = environment.apiUrl;

  newsletterConfig: NewsletterConfig;

  constructor(private http: HttpClient, private toastr: ToastrService, public datepipe: DatePipe, private router: Router) { }


  async addToUserNewsletterConfig(newsletterConfig: NewsletterConfig) {
    console.log("Šaljem sa " + newsletterConfig.favoriteData + ", " + newsletterConfig.walletData + " - " + newsletterConfig.frequency);
    await this.http.post(this.baseUrl + 'Newsletter', newsletterConfig).toPromise();

  }


  async getUserNewsletterConfig() {
    const response = await this.http.get<NewsletterConfig>(this.baseUrl + 'Newsletter').toPromise();
    this.newsletterConfig = response;
    console.log(this.newsletterConfig);
    return this.newsletterConfig;
  }
}
