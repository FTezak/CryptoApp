import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BinanceKeys } from 'src/app/_models/binanceKeys';
import { Cryptos } from 'src/app/_models/cryptos';
import { WalletDto } from 'src/app/_models/walletDto';
import { CryptoService } from 'src/app/_services/crypto.service';
import { FormControl, FormGroup } from '@angular/forms';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { WalletService } from 'src/app/_services/wallet.service';
import { Observable } from 'rxjs';
import {debounceTime, distinctUntilChanged, map} from 'rxjs/operators';

@Component({
  selector: 'app-wallet-modal',
  templateUrl: './wallet-modal.component.html',
  styleUrls: ['./wallet-modal.component.css']
})
export class WalletModalComponent implements OnInit {

  @Input() walletEntery = new EventEmitter();
  entery: WalletDto = {amount : 0, cryptoSym: ""};
  amount: number;


  cryptos: Cryptos[];
  cryptoSym: string = null;

  cryptoPicked: boolean = false;

  cryptoCurrentAmount: number;




  stateCtrl = new FormControl();
 
  myForm = new FormGroup({
    state: this.stateCtrl
  });

  constructor(public bsModalRef: BsModalRef, public cryptoService: CryptoService, public walletService: WalletService) { }

  async ngOnInit() {
    this.cryptos = await this.cryptoService.getAllCryptos();
  }

  addToWallet() {
    console.log(this.amount + ", " + this.cryptoSym);
    this.entery.amount = this.amount;
    this.entery.cryptoSym = this.cryptoSym;
    console.log(this.entery)
    this.walletEntery.emit(this.entery);
    this.bsModalRef.hide();
  }


  async onSelect(event: TypeaheadMatch) {
    this.cryptoSym = event.item.symbol;
    this.cryptoPicked = true;
    this.cryptoCurrentAmount = await this.walletService.getCurrentAmountForCrypto(this.cryptoSym);
  }


}
