import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HandleError, HttpErrorHandler } from '../http-error-handler.service';
import { Cart } from './cart';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Accept': 'text/plain',
    'Access-Control-Allow-Origin': 'https://localhost:44316',
    'Access-Control-Allow-Methods': 'GET, POST, PATCH, PUT, DELETE, OPTIONS',
    'Access-Control-Allow-Credentials': 'true',
    'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token,Accept'
  })
};
@Injectable({
  providedIn: 'root'
})
export class CartService {

  heroesUrl = 'api/Carts';  // URL to web api
  private handleError: HandleError;
  baseUrl = "";


  constructor(
    private http: HttpClient,
    httpErrorHandler: HttpErrorHandler, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    //console.log("BaeURL:" + baseUrl);
    this.handleError = httpErrorHandler.createHandleError('CartService');
  }

  /** GET heroes from the server */
  getHeroes(userid: string): Observable<Cart[]> {
    const url = `${this.baseUrl+this.heroesUrl}/${userid}`;
    return this.http.get<Cart[]>(url)
      .pipe(
        catchError(this.handleError('getHeroes', []))
      );
  }
  /** GET heroes from the server */
  getCartCount(userid: string): Observable<number> {
    const url = `${this.baseUrl+this.heroesUrl}/GetCartCount/${userid}`;
    return this.http.get<number>(url)
      .pipe(
        catchError(this.handleError('getHeroes', 0))
      );
  }
  getHeroesByID(userid: string, cartid: string): Observable<Cart[]> {
    const url = `${this.baseUrl+this.heroesUrl}/GetCartByID/${userid}/${cartid}`;
    return this.http.get<Cart[]>(url)
      .pipe(
        catchError(this.handleError('getHeroes', []))
      );
  }
  /** POST: add a new hero to the server */
  addHeroX(hero: Cart): Observable<Cart> {
    // const url = `${this.heroesUrl}/${hero.userID}`;
    return this.http.post<Cart>(this.baseUrl +this.heroesUrl, hero, httpOptions)
      .pipe(
        catchError(this.handleError('addHero', hero))
      );
  }
  deleteProductfromCart(hero: Cart): Observable<Cart> {
    // const url = `${this.heroesUrl}/${hero.userID}`;
    return this.http.post<Cart>(this.baseUrl +this.heroesUrl +'/DeleteProduct', hero, httpOptions)
      .pipe(
        catchError(this.handleError('addHero', hero))
      );
  }
  checkoutCart(hero: Cart): Observable<Cart> {
    // const url = `${this.heroesUrl}/${hero.userID}`;
    return this.http.post<Cart>(this.baseUrl +this.heroesUrl + '/Checkout', hero, httpOptions)
      .pipe(
        catchError(this.handleError('addHero', hero))
      );
  }
}
