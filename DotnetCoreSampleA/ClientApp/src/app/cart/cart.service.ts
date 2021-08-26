import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
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

  heroesUrl = 'https://localhost:44316/api/Carts';  // URL to web api
  private handleError: HandleError;


  constructor(
    private http: HttpClient,
    httpErrorHandler: HttpErrorHandler) {
    this.handleError = httpErrorHandler.createHandleError('CartService');
  }

  /** GET heroes from the server */
  getHeroes(userid: string): Observable<Cart[]> {
    const url = `${this.heroesUrl}/${userid}`;
    return this.http.get<Cart[]>(url)
      .pipe(
        catchError(this.handleError('getHeroes', []))
      );
  }
}
