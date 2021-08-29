import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HandleError, HttpErrorHandler } from '../http-error-handler.service';
import { Categories } from './Categories';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Accept':'text/plain',
    'Access-Control-Allow-Origin': 'https://localhost:44316',
    'Access-Control-Allow-Methods': 'GET, POST, PATCH, PUT, DELETE, OPTIONS',
    'Access-Control-Allow-Credentials': 'true',
    'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token,Accept'
  })
};

@Injectable({
  providedIn: 'root'  // <- ADD THIS
})
@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  heroesUrl = 'api/categories';  // URL to web api
  private handleError: HandleError;
  baseUrl = "";

  constructor(
    private http: HttpClient,
    httpErrorHandler: HttpErrorHandler, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.handleError = httpErrorHandler.createHandleError('CategoriesService');
  }

  /** GET heroes from the server */
  getHeroes(): Observable<Categories[]> {
    return this.http.get<Categories[]>(this.baseUrl+this.heroesUrl)
      .pipe(
        catchError(this.handleError('getHeroes', []))
      );
  }

  /** POST: add a new hero to the database */
  addHero(hero: Categories): Observable<Categories> {
    return this.http.post<Categories>(this.baseUrl +this.heroesUrl, hero, httpOptions)
      .pipe(
        catchError(this.handleError('addHero', hero))
      );
  }

  /** POST: add a new hero to the server */
  addHeroX(hero: Categories): Observable<Categories> {
    return this.http.post<Categories>(this.baseUrl +this.heroesUrl, hero, httpOptions)
      .pipe(
        catchError(this.handleError('addHero', hero))
      );
  }


  /** PUT: update the hero on the server. Returns the updated hero upon success. */
  updateHero(hero: Categories): Observable<Categories> {
    // httpOptions.headers =
    //   httpOptions.headers.set('Authorization', 'my-new-auth-token');
    const url = `${this.baseUrl +this.heroesUrl}/${hero.cat_id}`; // PUT api/heroes/42
    return this.http.put<Categories>(url, hero, httpOptions)
      .pipe(
        catchError(this.handleError('updateHero', hero))
      );
  }

  /** DELETE: delete the hero from the server */
  deleteHero(id: number): Observable<{}> {
    const url = `${this.baseUrl +this.heroesUrl}/${id}`; // DELETE api/heroes/42
    return this.http.delete(url, httpOptions)
      .pipe(
        catchError(this.handleError('deleteHero'))
      );
  }
}
