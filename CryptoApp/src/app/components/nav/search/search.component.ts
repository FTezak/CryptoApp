import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Cryptos } from 'src/app/_models/cryptos';
import { CryptoService } from 'src/app/_services/crypto.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  searchResults: Cryptos[];
  numberOfLettersTypedIn: number;

  constructor(public cryptoService: CryptoService, private router: Router) { }

  ngOnInit(): void {
  }


  async search(name: string){
    this.numberOfLettersTypedIn = name.length;
    if(name == "")
    {
      this.searchResults = await this.cryptoService.getAllCryptos();
    }
    else
    {
      this.searchResults = await this.cryptoService.getSearchCryptos(name);
    }

  }

  emptyResolts() {
    this.searchResults = null;
  }

  goto(crypto: string)
  {
    this.searchResults = null;
    this.router.navigate(['/crypto', crypto]);
  }

}
