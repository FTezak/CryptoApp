import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { CryptoCompareApi } from '../_models/cryptoCompareApi';
import { CryptoCurrentData } from '../_models/cryptoCurrentData';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { Cryptos } from '../_models/cryptos';
import { Wallet } from '../_models/wallet';
import { BinanceKeys } from '../_models/binanceKeys';
import { WalletDto } from '../_models/walletDto';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class WalletService {

  baseUrl = environment.apiUrl;

  wallet: Wallet;
  currentAmount: number;

  keys: BinanceKeys = {key : "", secretKey: ""};

  constructor(private http: HttpClient, private toastr: ToastrService, public datepipe: DatePipe, private router: Router) { }

  async getCurrentAmountForCrypto(crypto: string) {
    const response = await this.http.get<number>(this.baseUrl + 'Wallet/' + crypto).toPromise();

    console.log("trenutno ima " + crypto + ":" + response);
    return response;
  }

  async getUserWallet() {
    const response = await this.http.get<Wallet>(this.baseUrl + 'Wallet').toPromise();
    this.wallet = response;
    console.log(this.wallet);
    return this.wallet;
  }

  async addBinanceKeys(keys: BinanceKeys) {
    console.log("Šaljem sa " + keys.key + ", " + keys.secretKey)
    await this.http.post(this.baseUrl + 'Wallet/BinanceKeys', keys).toPromise();
    window.location.reload();
    this.getUserWallet();
  }

  async disconnectWithBinance() {
    console.log("Šaljem sa " + this.keys.key + ", " + this.keys.secretKey)
    await this.http.post(this.baseUrl + 'Wallet/BinanceKeys', this.keys).toPromise();
    window.location.reload();
    this.getUserWallet();
  }

  async addToWallet(wallet: WalletDto) {
    console.log("Šaljem sa " + wallet.cryptoSym + ", " + wallet.amount)
    await this.http.post(this.baseUrl + 'Wallet', wallet).toPromise();
    window.location.reload();

  }
}
