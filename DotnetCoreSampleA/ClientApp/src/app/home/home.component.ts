import { Component, Inject, OnInit } from '@angular/core';
import { Product } from '../products/Product';
import { HomeService } from './home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  products: Product[];
  constructor(private heroesService: HomeService, @Inject('BASE_URL') baseUrl: string) {
    console.log("BaeURL:" + baseUrl);
  }

  ngOnInit() {
    this.getProdcuts();
  }
  public createImgPath = (serverPath: string) => {
    return 'https://localhost:44316/' + serverPath;
  }
  getProdcuts(): void {
    this.heroesService.getHeroes()
      .subscribe(heroes => {
        (this.products = heroes);
        //  console.log(JSON.stringify(this.products));
      });
  }
}
