import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-forbidden',
  templateUrl: './forbidden.component.html',
  styleUrls: ['./forbidden.component.css']
})
export class ForbiddenComponent implements AfterViewInit {
  @ViewChild('myDiv', { static: false }) myDiv: ElementRef;
  constructor() { }

  ngAfterViewInit() {
    var str = this.myDiv.nativeElement.innerHTML.toString();
    var i = 0;
    this.myDiv.nativeElement.innerHTML = "";
    let ix = 1;
    setTimeout(() => {
      var se = setInterval(() => {
        ix++;
        this.myDiv.nativeElement.innerHTML = str.slice(0, ix) + "|";
        if (ix == str.length) {
          clearInterval(se);
          this.myDiv.nativeElement.innerHTML = str;
        }
      }, 5);
    }, 0);

   /* setTimeout(() => {
      console.log('hide');
      this.myDiv.nativeElement.innerHTML = "TAYYAB";
    }, 2000);*/

   /* setTimeout(function () {

      var se = setInterval(function () {
        i++;
        this.myDiv.nativeElement.innerHTML = str.slice(0, i) + "|";
        if (i == str.length) {
          clearInterval(se);
          this.myDiv.nativeElement.innerHTML = str;
        }
      }, 10);
    }, 0);*/
  }

}
