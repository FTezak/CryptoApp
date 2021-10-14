import { Component, OnInit } from '@angular/core';
import { NewsletterConfig } from 'src/app/_models/newsletterConfig';
import { NewsletterService } from 'src/app/_services/newsletter.service';

@Component({
  selector: 'app-newsletter',
  templateUrl: './newsletter.component.html',
  styleUrls: ['./newsletter.component.css'],
  styles: [
    `
      :host >>> .tooltip-inner {
        background-color: #F7931A;
        color: #212529;
        font-weight: bold;
      }
      :host >>> .tooltip.top .tooltip-arrow:before,
      :host >>> .tooltip.top .tooltip-arrow {
        border-top-color: #F7931A;
      }
    `
  ]
})
export class NewsletterComponent implements OnInit {

  
  html4 = `00:00 <br> 06:00 <br> 12:00 <br> 18:00`;
  html8 = `00:00 <br> 03:00 <br> 06:00 <br> 09:00 <br> 12:00 <br> 15:00 <br> 18:00 <br> 21:00`;

  newsletterConfig: NewsletterConfig;
  originalNewsletterConfig: NewsletterConfig;

  dirtyData = false;

  radioModel: string;

  isChecked = false;

  checkModel: { wallet?: boolean; favorites?: boolean } = { wallet: true, favorites: true };

  constructor(public newsletterService: NewsletterService) { }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    this.originalNewsletterConfig = await this.newsletterService.getUserNewsletterConfig();

    if(this.originalNewsletterConfig.frequency == 0)
    {
      this.newsletterConfig = {...this.originalNewsletterConfig};
      this.isChecked = false
      this.radioModel = "1";
      this.checkModel.wallet = false;
      this.checkModel.favorites = true;
      this.dirtyData = true;
    }
    else
    {
      this.newsletterConfig = {...this.originalNewsletterConfig};
      this.isChecked = true
      this.radioModel = this.originalNewsletterConfig.frequency.toString();
      this.checkModel.wallet = this.originalNewsletterConfig.walletData;
      this.checkModel.favorites = this.originalNewsletterConfig.favoriteData;
      this.dirtyData = false;
    }

    
  }

  async addToUserNewsletterConfig() {
    this.newsletterConfig.favoriteData = this.checkModel.favorites;
    this.newsletterConfig.walletData = this.checkModel.wallet;
    this.newsletterConfig.frequency = parseInt(this.radioModel);

    await this.newsletterService.addToUserNewsletterConfig(this.newsletterConfig);
    console.log("TUUUUUU!");

  }

  async removeUserNewsletterConfig() {

    if(this.isChecked)
    {
      this.newsletterConfig.favoriteData = false;
      this.newsletterConfig.walletData = false;
      this.newsletterConfig.frequency =0;
  
      await this.newsletterService.addToUserNewsletterConfig(this.newsletterConfig);
    }
    else{
      this.checkModel.wallet = false;
      this.checkModel.favorites = true;
      this.radioModel = "1";
      this.dirtyData = true;
    }
    
  }

  clicked() {
    this.newsletterConfig.favoriteData = this.checkModel.favorites;
    this.newsletterConfig.walletData = this.checkModel.wallet;
    this.newsletterConfig.frequency = parseInt(this.radioModel);

    console.log("N -> " + this.newsletterConfig.favoriteData + " - " + this.newsletterConfig.walletData + " - " +  this.newsletterConfig.frequency);
    console.log("O -> " + this.originalNewsletterConfig.favoriteData + " - " + this.originalNewsletterConfig.walletData + " - " +  this.originalNewsletterConfig.frequency);
 
    if(this.newsletterConfig.favoriteData != this.originalNewsletterConfig.favoriteData || this.newsletterConfig.walletData != this.originalNewsletterConfig.walletData || this.newsletterConfig.frequency != this.originalNewsletterConfig.frequency)
    {
      this.dirtyData = true;
    }
    else
    {
      this.dirtyData = false;
    }
    
    
  }

}
