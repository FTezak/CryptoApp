import { Component, OnInit } from '@angular/core';
import { CryptoService } from 'src/app/_services/crypto.service';
import { DatePipe } from '@angular/common';

import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_dark from "@amcharts/amcharts4/themes/dark";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { CryptoCompareApi } from 'src/app/_models/cryptoCompareApi';

import { ActivatedRoute } from '@angular/router';
import { CryptoCurrentData } from 'src/app/_models/cryptoCurrentData';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';
import { any } from '@amcharts/amcharts4/.internal/core/utils/Array';

// am4core.useTheme(am4themes_dark);
am4core.useTheme(am4themes_animated);




@Component({
  selector: 'app-crypto-info',
  templateUrl: './crypto-info.component.html',
  styleUrls: ['./crypto-info.component.css'],
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
export class CryptoInfoComponent implements OnInit {

  cryptoCurrency: string = 'BTC';
  cryptoPricesByMinute: CryptoCompareApi[];

  dataForGraph: CryptoCompareApi[];

  //emptyData: CryptoCompareApi[] = [];

  cryptosCurrentData: CryptoCurrentData;

  crypto: string;
  favorite: boolean;
  msg: string;



  radioModel = 'all';


  constructor(public cryptoService: CryptoService, public datepipe: DatePipe, private _Activatedroute: ActivatedRoute, public toastr: ToastrService, public accountService: AccountService) { }

  async ngOnInit() {

    this._Activatedroute.paramMap.subscribe(params => {
      this.crypto = params.get('crypto');
    });

    this.accountService.currentUser$.subscribe(e => {
      if(e != null)
      {
        this.cryptoService.isCryptoFavorite(this.crypto).subscribe(x => {
          this.favorite = x;
        });
      }
    })

    

    

    this.cryptoService.getCryptos();

    this.cryptoPricesByMinute = await this.cryptoService.getCryptosDetail(this.crypto);
    this.dataForGraph = this.cryptoPricesByMinute;

    this.loadData()
    this.amChartsInit();
  }


  async loadData(){
    this.cryptosCurrentData = await this.cryptoService.getCryptosCurrentPriceBySymbol(this.crypto);
  }

  addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
  }
  

  updateData(){

    if(this.radioModel == "all")
    {
      this.dataForGraph = this.cryptoPricesByMinute;
      this.amChartsInit();
    }
    if(this.radioModel == "day")
    {
      
      const date = new Date();
      date.setDate(date.getDate() - 1);

      var milliseconds = date.getTime();

      console.log(date);
      console.log(this.dataForGraph);
      this.dataForGraph = new Array();
      console.log(this.dataForGraph);
      this.cryptoPricesByMinute.forEach((object) => {

        var s = date.toISOString();

        var x = object.time.toString();

        var d1 = new Date(x);
        var d2 = new Date(s);

        if(d1 > d2)
        {
          this.dataForGraph.push(object);
        }
      })
      console.log(this.dataForGraph);
      this.amChartsInit();
    }
    if(this.radioModel == "week")
    {
      const date = new Date();
      date.setDate(date.getDate() - 7);

      var milliseconds = date.getTime();

      console.log(date);
      console.log(this.dataForGraph);
      this.dataForGraph = new Array();
      console.log(this.dataForGraph);
      this.cryptoPricesByMinute.forEach((object) => {

        var s = date.toISOString();

        var x = object.time.toString();

        var d1 = new Date(x);
        var d2 = new Date(s);

        if(d1 > d2)
        {
          this.dataForGraph.push(object);
        }
      })
      console.log(this.dataForGraph);
      this.amChartsInit();
    }
    if(this.radioModel == "month")
    {
     

      const date = new Date();
      date.setDate(date.getDate() - 31);

      var milliseconds = date.getTime();

      console.log(date);
      console.log(this.dataForGraph);
      
      this.dataForGraph = new Array();
      console.log(this.dataForGraph);
      this.cryptoPricesByMinute.forEach((object) => {


        var s = date.toISOString();

        var x = object.time.toString();

        var d1 = new Date(x);
        var d2 = new Date(s);

        if(d1 > d2)
        {
          this.dataForGraph.push(object);
        }
      })
      console.log(this.dataForGraph);
      this.amChartsInit();
    }
  }


  amChartsInit() {
    let chart = am4core.create("chartdiv", am4charts.XYChart);

    chart.dateFormatter.inputDateFormat = "dd/MM/yyyy HH:mm";
    chart.data = this.dataForGraph;


    let dateAxis = chart.xAxes.push(new am4charts.DateAxis());
    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

    // Create series
    let series = chart.series.push(new am4charts.LineSeries());
    series.dataFields.valueY = "open";
    series.dataFields.dateX = "time";
    series.tooltipText = "{open}"
    series.strokeWidth = 2;
    series.minBulletDistance = 15;

    // Drop-shaped tooltips
    series.tooltip.background.cornerRadius = 20;
    series.tooltip.background.strokeOpacity = 0;
    series.tooltip.pointerOrientation = "vertical";
    series.tooltip.label.minWidth = 40;
    series.tooltip.label.minHeight = 40;
    series.tooltip.label.textAlign = "middle";
    series.tooltip.background.fillOpacity = 0.5;

    // Make bullets grow on hover
    let bullet = series.bullets.push(new am4charts.CircleBullet());
    bullet.circle.strokeWidth = 2;
    bullet.circle.radius = 5;
    bullet.circle.fill = am4core.color("#fff");

    let bullethover = bullet.states.create("hover");
    bullethover.properties.scale = 1.3;

    // Make a panning cursor
    chart.cursor = new am4charts.XYCursor();
    //chart.cursor.behavior = "panXY";
    chart.cursor.xAxis = dateAxis;
    chart.cursor.snapToSeries = series;

    // Create a horizontal scrollbar with previe and place it underneath the date axis
    let scrollbarX = new am4charts.XYChartScrollbar();
    scrollbarX.series.push(series);
    chart.scrollbarX = scrollbarX;
    chart.scrollbarX.parent = chart.bottomAxesContainer;

    chart.cursor.lineX.stroke = am4core.color("#8F3985");
    chart.cursor.lineX.strokeWidth = 2;
    //chart.cursor.lineX.strokeOpacity = 0.2;
    chart.cursor.lineX.strokeDasharray = "";

    chart.cursor.lineY.stroke = am4core.color("#8F3985");
    chart.cursor.lineY.strokeWidth = 2;
    //chart.cursor.lineY.strokeOpacity = 0.2;
    chart.cursor.lineY.strokeDasharray = "";

    dateAxis.start = 0.79;
    dateAxis.keepSelection = false;
    dateAxis.groupData = false;
    dateAxis.groupCount = 500;
  }


 

  addToFavorite(crypto: string) {


    if (this.favorite) {
      this.favorite = false;
      this.cryptosCurrentData.favorite = false;
      this.msg = 'You removed ' + crypto + ' to favorite';
    }
    else {
      this.favorite = true;
      this.cryptosCurrentData.favorite = true;
      this.msg = 'You added ' + crypto + ' to favorite';
    }
    this.cryptoService.addToFAvorite(crypto).subscribe(() => {
      this.toastr.success(this.msg);
    });
  }

}


