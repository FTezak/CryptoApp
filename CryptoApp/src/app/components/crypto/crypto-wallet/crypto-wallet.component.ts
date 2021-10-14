import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { BinanceKeys } from 'src/app/_models/binanceKeys';
import { Wallet } from 'src/app/_models/wallet';
import { WalletDto } from 'src/app/_models/walletDto';
import { WalletService } from 'src/app/_services/wallet.service';
import { BinaceKeysComponent } from '../../modals/binace-keys/binace-keys.component';
import { WalletModalComponent } from '../../modals/wallet-modal/wallet-modal.component';

@Component({
  selector: 'app-crypto-wallet',
  templateUrl: './crypto-wallet.component.html',
  styleUrls: ['./crypto-wallet.component.css'],
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
export class CryptoWalletComponent implements OnInit {

  wallet: Wallet;

  bsModalRef: BsModalRef;

  keys: BinanceKeys = {key : "", secretKey: ""};

  entery: WalletDto = {amount : 0, cryptoSym: ""};

  constructor(public walletService: WalletService, public modalService: BsModalService) { }

  async ngOnInit() {

      await this.loadData();
  }

  async loadData(){
    this.wallet = await this.walletService.getUserWallet();
  }

  openBinanceKeysModal() {

    const config = {
      class: 'modal-dialog-centered'

    }

    this.bsModalRef = this.modalService.show(BinaceKeysComponent, config);
    this.bsModalRef.content.binanceKeys.subscribe(values => {
      console.log("Primio: " + values)
      this.keys = values;
      console.log("Primio2: " + this.keys.key)
      this.walletService.addBinanceKeys(this.keys);
    })
  }


  openWalletModal() {

    const config = {
      class: 'modal-dialog-centered'

    }

    this.bsModalRef = this.modalService.show(WalletModalComponent, config);
    this.bsModalRef.content.walletEntery.subscribe(values => {
      console.log("Primio: " + values)
      this.entery = values;
      console.log("Primio2: " + this.entery.amount + ", " + this.entery.cryptoSym)
      this.walletService.addToWallet(this.entery);
    })
  }

  disconnectWithBinance() {
    this.walletService.disconnectWithBinance();
  }

}
