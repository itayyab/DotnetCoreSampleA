import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable()
export class SharedService {
  isChanged: boolean;
  isLoginChnaged: boolean;

  cartChange: Subject<boolean> = new Subject<boolean>();
  loginChange: Subject<boolean> = new Subject<boolean>();

  constructor() {
    this.cartChange.subscribe((value) => {
      this.isChanged = value
    });
    this.loginChange.subscribe((value) => {
      this.isLoginChnaged = value
    });
  }

  togglecartChange() {
    this.cartChange.next(!this.isChanged);
  }
  toggleloginChange() {
    this.cartChange.next(!this.isChanged);
  }
}
