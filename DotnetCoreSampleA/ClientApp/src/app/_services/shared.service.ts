import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable()
export class SharedService {
  isChanged: boolean;

 cartChange: Subject<boolean> = new Subject<boolean>();

  constructor() {
    this.cartChange.subscribe((value) => {
      this.isChanged = value
    });
  }

  toggleChange() {
    this.cartChange.next(!this.isChanged);
  }
}
