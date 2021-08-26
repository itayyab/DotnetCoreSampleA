import { Component, Inject, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { Cart } from './cart';
import { CartService } from './cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  // public userSub: Observable<string>;
  heroes: Cart[];
  constructor(private heroesService: CartService, @Inject('BASE_URL') baseUrl: string, private authorizeService: AuthorizeService) {
    console.log("BaeURL:" + baseUrl);
  }
  ngOnInit() {
    
    this.authorizeService.getUserSub().subscribe(heroes => {
      console.log("userSub:" + heroes);
      this.getHeroes(heroes);

      //(this.products = heroes);
      //  console.log(JSON.stringify(this.products));
    });
  }

  getHeroes(userid:string): void {
    this.heroesService.getHeroes(userid)
      .subscribe(heroes => {
        console.log(JSON.stringify(heroes));
        this.heroes = heroes;
      });
  }
  public createImgPath = (serverPath: string) => {
    return 'https://localhost:44316/' + serverPath;
  }
  public getsubtotal(v1, v2): number {
    console.log(v1);
    return v1 * v2;
  }
}
