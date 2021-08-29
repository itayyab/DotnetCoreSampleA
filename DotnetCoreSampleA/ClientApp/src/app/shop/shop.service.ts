import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HandleError, HttpErrorHandler } from '../http-error-handler.service';
import { Product } from '../products/Product';

@Injectable({
  providedIn: 'root'
})
export class ShopService {

  private handleError: HandleError;
  heroesUrl = 'api/Products/WithCategory';
  baseUrl = "";
  constructor(
    private http: HttpClient,
    httpErrorHandler: HttpErrorHandler, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.handleError = httpErrorHandler.createHandleError('CategoriesService');
  }
  /** GET heroes from the server */
  /** GET heroes from the server */
  getHeroes(): Observable<Product[]> {
    return this.http.get<Product[]>(this.baseUrl+this.heroesUrl)
      .pipe(
        catchError(this.handleError('getHeroes', []))
      );
  }
}
