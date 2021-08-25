import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HandleError, HttpErrorHandler } from '../http-error-handler.service';
import { Product } from '../products/Product';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  private handleError: HandleError;
  heroesUrlUp = 'https://localhost:44316/api/products/UploadFile';  // URL to web api
  heroesUrl = 'https://localhost:44316/api/Products/WithCategory'
  constructor(
    private http: HttpClient,
    httpErrorHandler: HttpErrorHandler) {
    this.handleError = httpErrorHandler.createHandleError('CategoriesService');
  }
  /** GET heroes from the server */
  /** GET heroes from the server */
  getHeroes(): Observable<Product[]> {
    return this.http.get<Product[]>(this.heroesUrl)
      .pipe(
        catchError(this.handleError('getHeroes', []))
      );
  }
}
