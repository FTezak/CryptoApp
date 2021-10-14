import { Component, OnInit } from '@angular/core';
import { CryptoService } from 'src/app/_services/crypto.service';
import { DatePipe } from '@angular/common';
import { CryptoCompareApi } from 'src/app/_models/cryptoCompareApi';
import { CryptoCurrentData } from 'src/app/_models/cryptoCurrentData';
import { Pagination } from 'src/app/_models/pagination';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-crypto-home',
  templateUrl: './crypto-home.component.html',
  styleUrls: ['./crypto-home.component.css'],
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
export class CryptoHomeComponent implements OnInit {

  cryptoCurrency: string = 'BTC';

  cryptoPricesByMinute: CryptoCompareApi[];

  cryptosCurrentData: CryptoCurrentData[];
  selectedCrypto: CryptoCurrentData;
  msg: string;
  pagination: Pagination;
  pageNumber = 1;
  pageSize = 10;
  uploading: boolean = false;

  constructor(public cryptoService: CryptoService, public datepipe: DatePipe, public toastr: ToastrService, public accountService: AccountService) { }

  async ngOnInit() {

    this.loadData();
  }

  loadData(){
    this.uploading = true;
    this.cryptoService.getCryptosCurrentPrice(this.pageNumber, this.pageSize).subscribe(response => {
      this.cryptosCurrentData = response.result;
      this.pagination = response.pagination;
      this.uploading = false;
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadData();
  }

  addToFavorite(crypto: CryptoCurrentData){
    
    this.selectedCrypto = this.cryptosCurrentData.find(x => x.symbol == crypto.symbol);
    if(this.selectedCrypto.favorite)
    {
      this.cryptosCurrentData.find(x => x.symbol == crypto.symbol).favorite = false;
      this.msg = 'You removed ' + crypto.symbol + ' from favorite';
    }
    else{
      this.cryptosCurrentData.find(x => x.symbol == crypto.symbol).favorite = true;
      this.msg = 'You added ' + crypto.symbol + ' to favorite';
    }
    this.cryptoService.addToFAvorite(crypto.symbol).subscribe(() => {
      this.toastr.success(this.msg);
    });

  }
}


