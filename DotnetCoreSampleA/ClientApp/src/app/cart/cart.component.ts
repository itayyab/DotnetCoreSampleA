import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { SharedService } from '../_services/shared.service';
import { ToastService } from '../_services/toast.service';
import { UserService } from '../_services/user.service';
import { Cart } from './cart';
import { CartDetails } from './cart-details';
import { CartService } from './cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  // public userSub: Observable<string>;
  heroes: Cart[];
  userid: string;
  baseUrl = "";
  constructor(private heroesService: CartService, @Inject('BASE_URL') baseUrl: string, private authorizeService: UserService, public toastService: ToastService, private route: ActivatedRoute,
    private router: Router, private sharedService: SharedService) {
    this.baseUrl = baseUrl;
    //console.log("BaeURL:" + baseUrl);
  }
  ngOnInit() {
    
    this.authorizeService.getHeroesXX().subscribe(heroesX => {
      var heroes = heroesX.body.userId;
     // console.log("userSub:" + heroes);
      this.userid = heroes;
      this.getHeroes(heroes);

      //(this.products = heroes);
      //  console.log(JSON.stringify(this.products));
    });
  }

  getHeroes(userid:string): void {
    this.heroesService.getHeroes(userid)
      .subscribe(heroes => {
       // console.log(JSON.stringify(heroes));
        this.heroes = heroes;
      });
  }
  delfromCart(productid: number,product:string): void {
    this.authorizeService.getHeroesXX().subscribe(heroesX => {
      var heroes = heroesX.body.userId;
    //  console.log("userSub:" + heroes);

      var carx: CartDetails[] = [{ cD_Pr_id: productid, cD_id: 0, cartForeignKey: 0, cD_Pr_Amnt: 0, cD_Pr_price: 0, cD_Pr_Qty: 0, cart: null, product: null, productForeignKey: 0 }];
      var cart = {
        cart_id: 0, status: "PENDING", totalAmount: 0, totalQty: 0, userID: heroes, cartDetails: carx
      } as Cart;
      console.log(JSON.stringify(cart));
      this.heroesService.deleteProductfromCart(cart)
        .subscribe(hero => {
       //   console.log(JSON.stringify(hero));
          this.showError(product);
          this.sharedService.togglecartChange();
       //   console.log('size' + this.heroes.length);
        //  var cx;
        /*  this.heroes.filter(obj => obj.cartDetails !== undefined)
            .forEach(item => { if (item.cartDetails) { item.items = this.filterArray(item.items) } };*/
         // var testing = this.heroes.filter(h => h.cartDetails.filter(item => item.cD_Pr_id !== productid));
          //console.log("TEST:" + JSON.stringify(testing));
          this.getHeroes(this.userid);


          //this.products = this.products.filter(h => h.pr_id !== hero.pr_id);
       //   console.log('size' + this.heroes.length);
          //console.log(hero.email);
          //  this.heroes.push(hero);
          //$(this.exampleModal.nativeElement).modal('hide');
          //  this.message = 'Category details added';
          //  $(this.toast.nativeElement).toast('show');
        });
    });
  }
  checkoutCart(): void {
    this.authorizeService.getHeroesXX().subscribe(heroesX => {
      var heroes = heroesX.body.userId;
      //console.log("userSub:" + heroes);
      var carx: CartDetails[] = [{ cD_Pr_id: 0, cD_id: 0, cartForeignKey: 0, cD_Pr_Amnt: 0, cD_Pr_price: 0, cD_Pr_Qty: 0, cart: null, product: null, productForeignKey: 0 }];
      var cart = {
        cart_id: 0, status: "CONFIRMED", totalAmount: 0, totalQty: 0, userID: heroes, cartDetails: carx
      } as Cart;
      //console.log(JSON.stringify(cart));
      this.heroesService.checkoutCart(cart)
        .subscribe(hero => {
         // console.log(JSON.stringify(hero));
          //  this.getHeroes(this.userid);
          this.showSuccess();
          this.sharedService.togglecartChange();
          this.router.navigate(['/checkout', { id: hero.cart_id }]);
        });
    });
  }
  showSuccess() {
    this.toastService.show("Completed successfully!", {
      classname: 'bg-success text-light',
      delay: 2000,
      autohide: true,
      headertext: 'Checkout'
    });
  }
  showError(product) {
    this.toastService.show(product, {
      classname: 'bg-danger text-light',
      delay: 2000,
      autohide: true,
      headertext: 'Product removed from cart'
    });
  }
  public gettotal(): number {
    var total = 0;
    if (this.heroes != null && this.heroes != undefined) {
      this.heroes.forEach(x => {
        total += x.totalAmount;
      });
    }
    return total;
  }

  public createImgPath = (serverPath: string) => {
    return this.baseUrl + serverPath;
  }
  public getsubtotal(v1, v2): number {
    //console.log(v1);
    return v1 * v2;
  }
}
