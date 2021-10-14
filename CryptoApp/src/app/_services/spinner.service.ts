import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  requestCount = 0;

  constructor(private spinnerService: NgxSpinnerService) { }

  busy() {
    this.requestCount++;
    this.spinnerService.show(undefined, {
      type: 'ball-running-dots',
      bdColor: 'rgba(255,255,255,0.5)',
      color: '#0D6EFD'
    });
  }

  idle(){
    this.requestCount--;
    if(this.requestCount <= 0){
      this.requestCount = 0;
      this.spinnerService.hide();
    }
  }
}
