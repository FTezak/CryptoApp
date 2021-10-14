import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BinanceKeys } from 'src/app/_models/binanceKeys';


@Component({
  selector: 'app-binace-keys',
  templateUrl: './binace-keys.component.html',
  styleUrls: ['./binace-keys.component.css']
})
export class BinaceKeysComponent implements OnInit {

  @Input() binanceKeys = new EventEmitter();
  keys: BinanceKeys = {key : "", secretKey: ""};
  key: string;
  secretKey: string;

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  addBinanceKeys() {
    console.log(this.key + ", " + this.secretKey);
    this.keys.key = this.key;
    this.keys.secretKey = this.secretKey;
    console.log(this.keys)
    this.binanceKeys.emit(this.keys);
    this.bsModalRef.hide();
  }

}
