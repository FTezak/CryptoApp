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

@Injectable({
  providedIn: 'root'
})
export class CryptoService {

  baseUrl = environment.apiUrl;
  baseUrlCryptocompare = environment.cryptocompareApiUrl;

  cryptos: any;
  cryptosByMinute: CryptoCompareApi[] = [];
  cryptosCurrentData: CryptoCurrentData[] = [];
  cryptosDetails: CryptoCompareApi[] = [];
  favoriteCryptos: Cryptos[] = [];
  searchCryptos: Cryptos[] = [];
  allCryptos: Cryptos[] = [];

  paginatedResult: PaginatedResult<CryptoCurrentData[]> = new PaginatedResult<CryptoCurrentData[]>();

  constructor(private http: HttpClient) { }


  getCryptos(){
      this.http.get(this.baseUrl + 'Crypto').subscribe(response => {
      this.cryptos = response;
    }, error => {
      console.log(error);
    })
  }

  getCryptosCurrentPrice(page?: number, itempPerPage?: number) {

    let params = new HttpParams();

    if(page !== null && itempPerPage !== null){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itempPerPage.toString());
    }

    return this.http.get<CryptoCurrentData[]>(this.baseUrl + 'Crypto/home', {observe: 'response', params}).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if(response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return this.paginatedResult
      })
    )
    
  }



  async getCryptosCurrentPriceBySymbol(crypto: string){

    const response = await this.http.get<CryptoCurrentData>(this.baseUrl + 'Crypto/home/' + crypto).toPromise();

    return response;
}


  async getCryptoByMinuteData(crypto: string, currency: string) {
    const response = await this.http.get<any>(this.baseUrlCryptocompare + 'histominute?fsym=' + crypto + '&tsym=' + currency + '&limit=1440&api_key=**********').toPromise();
    this.cryptosByMinute = response.Data.Data.map(data => {
      return {
        open: data.open.toFixed(8),
        time: new Date(data.time * 1000)
      } as CryptoCompareApi
    })
    console.log(this.cryptosByMinute);
    return this.cryptosByMinute;
  }

  async getCryptosDetail(crypto: string) {
    const response = await this.http.get<any>(this.baseUrl + 'Crypto/details/'+ crypto).toPromise();
    this.cryptosDetails = response.map(data => {
      return {
        open: data.open.toFixed(8),
        time: data.date
      } as CryptoCompareApi
    })
    console.log(this.cryptosDetails);
    return this.cryptosDetails;
  }

  async getFavoriteCryptos() {
    const response = await this.http.get<any>(this.baseUrl + 'Crypto/userfav').toPromise();
    this.favoriteCryptos = response.map(data => {
      return {
        cryptoApiReference: data.cryptoApiReference,
        symbol: data.symbol,
        name: data.name
      } as Cryptos
    })
    console.log(this.favoriteCryptos);
    return this.favoriteCryptos;
  }


  async getSearchCryptos(name: string) {
    const response = await this.http.get<any>(this.baseUrl + 'Crypto/searchCrypto/' + name).toPromise();
    this.searchCryptos = response.map(data => {
      return {
        cryptoApiReference: data.cryptoApiReference,
        symbol: data.symbol,
        name: data.name
      } as Cryptos
    })
    console.log(this.searchCryptos);
    return this.searchCryptos;
  }

  async getAllCryptos() {
    const response = await this.http.get<any>(this.baseUrl + 'Crypto/all').toPromise();
    this.allCryptos = response.map(data => {
      return {
        cryptoApiReference: data.cryptoApiReference,
        symbol: data.symbol,
        name: data.name
      } as Cryptos
    })
    console.log(this.allCryptos);
    return this.allCryptos;
  }

  getCryptosDetail2(crypto: string){
    
      this.http.get(this.baseUrl + 'Crypto/details/'+ crypto).subscribe(response => {
      //this.users = response;
    }, error => {
      console.log(error);
    })
  }



  addToFAvorite(crypto: string){
    return this.http.post(this.baseUrl + 'Crypto/' + crypto, {});
  }

  isCryptoFavorite(crypto: string){
    return this.http.post<boolean>(this.baseUrl + 'Crypto/fav/' + crypto, {});
  }
}
