import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { Cart } from '../cart/cart';
import { CartService } from '../cart/cart.service';
import { ToastService } from '../_services/toast.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {

  heroes: Cart[];
  userid: string;
  baseUrl = "";
  constructor(private heroesService: CartService, @Inject('BASE_URL') baseUrl: string, private authorizeService: AuthorizeService, public toastService: ToastService, private route: ActivatedRoute,
    private router: Router) {
    this.baseUrl = baseUrl;
   // console.log("BaeURL:" + baseUrl);
  }
  ngOnInit() {
    const heroId = this.route.snapshot.paramMap.get('id');
    //console.log("IDDDD1:" + heroId);
    /*this.route.paramMap.pipe(
      switchMap(params => {
        var selectedId = Number(params.get('id'));
        console.log("IDDDD:"+selectedId);
       // this.getHeroes(selectedId);
        return null;
       // return this.service.getHeroes();
      })
    );*/
    this.authorizeService.getUserSub().subscribe(heroes => {
     // console.log("userSub:" + heroes);
      this.userid = heroes;
      this.getHeroes(heroes, heroId);

      //(this.products = heroes);
      //  console.log(JSON.stringify(this.products));
    });
  }

  getHeroes(userid: string, cartid: string): void {
    this.heroesService.getHeroesByID(userid,cartid)
      .subscribe(heroes => {
       // console.log(JSON.stringify(heroes));
        this.heroes = heroes;
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
