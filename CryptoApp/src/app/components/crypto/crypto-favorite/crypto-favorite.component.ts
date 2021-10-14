import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Cryptos } from 'src/app/_models/cryptos';
import { AccountService } from 'src/app/_services/account.service';
import { CryptoService } from 'src/app/_services/crypto.service';

@Component({
  selector: 'app-crypto-favorite',
  templateUrl: './crypto-favorite.component.html',
  styleUrls: ['./crypto-favorite.component.css']
})
export class CryptoFavoriteComponent implements OnInit {

  favoriteCrypto: Cryptos[];

  constructor(public cryptoService: CryptoService, public datepipe: DatePipe, public toastr: ToastrService, public accountService: AccountService) { }

  async ngOnInit() {
    this.loadData()
  }

  async loadData(){
    this.favoriteCrypto = await this.cryptoService.getFavoriteCryptos();
  }

  addToFavorite(crypto: Cryptos){
    
    this.cryptoService.addToFAvorite(crypto.symbol).subscribe(() => {
      this.toastr.success('You removed ' + crypto.symbol + ' from favorite');
      this.loadData();
    });
    
  }

}
