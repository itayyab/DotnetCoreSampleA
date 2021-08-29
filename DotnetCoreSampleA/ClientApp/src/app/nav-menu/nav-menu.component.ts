import { Component, Inject, OnInit } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { CartService } from '../cart/cart.service';
import { SharedService } from '../_services/shared.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  constructor(private heroesService: CartService, @Inject('BASE_URL') baseUrl: string, private authorizeService: AuthorizeService, private sharedService: SharedService) {
    console.log("BaeURL:" + baseUrl);
  }
  count: number = 0;
  ngOnInit() {
    this.getCount();
    this.sharedService.cartChange.subscribe(value => {
      this.getCount();
    });
  }
  getCount() {
    this.authorizeService.getUserSub().subscribe(heroes => {
     // console.log("userSub:" + heroes);
      this.heroesService.getCartCount(heroes)
        .subscribe(heroes => {
          this.count = heroes;
        });

      //(this.products = heroes);
      //  console.log(JSON.stringify(this.products));
    });

  }
  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
