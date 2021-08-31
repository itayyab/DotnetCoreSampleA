import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { Cart } from '../cart/cart';
import { CartDetails } from '../cart/cart-details';
import { CartService } from '../cart/cart.service';
import { Product } from '../products/Product';
import { ShopService } from './shop.service';
import $ from 'jquery';
import { ToastService } from '../_services/toast.service';
import { SharedService } from '../_services/shared.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit {

  products: Product[];
  baseUrl = "";
  constructor(private heroesService: ShopService, private cartService: CartService, @Inject('BASE_URL') baseUrl: string, private authorizeService: AuthorizeService, public toastService: ToastService, private sharedService: SharedService) {
    this.baseUrl = baseUrl;
  //  console.log("BaeURL:" + baseUrl);
  }

  ngOnInit() {
    this.getProdcuts();
  }
  public createImgPath = (serverPath: string) => {
    return this.baseUrl + serverPath;
  }
  getProdcuts(): void {
    this.heroesService.getHeroes()
      .subscribe(heroes => {
        (this.products = heroes);
         // console.log(JSON.stringify(this.products));
      });
  }
  addtoCart(productid: number, name: string): void {
    this.authorizeService.getUserSub().subscribe(heroes => {
      //console.log("userSub:" + heroes);
      var carx: CartDetails[] = [{ cD_Pr_id: productid, cD_id:0, cartForeignKey: 0, cD_Pr_Amnt: 0, cD_Pr_price: 0, cD_Pr_Qty: 0, cart: null, product: null, productForeignKey:0 }];
      var cart = {
        cart_id: 0, status: "PENDING", totalAmount: 0, totalQty: 0, userID: heroes, cartDetails: carx
      } as Cart;
     // console.log(JSON.stringify(cart));
      this.cartService.addHeroX(cart)
        .subscribe(hero => {
          this.showSuccess(name);
          this.sharedService.toggleChange();
      });
    });
  }
  showSuccess(product) {
    this.toastService.show(product, {
      classname: 'bg-success text-light',
      delay: 5000,
      autohide: true,
      headertext: 'Product added to cart'
    });
  }
}
