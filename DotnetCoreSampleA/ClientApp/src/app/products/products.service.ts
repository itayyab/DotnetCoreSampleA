import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HandleError, HttpErrorHandler } from '../http-error-handler.service';
import { Product } from './Product';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'multipart/form-data',
    'Access-Control-Allow-Origin': 'https://localhost:44316',
    'Access-Control-Allow-Methods': 'GET, POST, PATCH, PUT, DELETE, OPTIONS',
    'Access-Control-Allow-Credentials': 'true',
    'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token'
  })
};
const httpOptions2 = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'Access-Control-Allow-Origin': 'https://localhost:44316',
    'Access-Control-Allow-Methods': 'GET, POST, PATCH, PUT, DELETE, OPTIONS',
    'Access-Control-Allow-Credentials': 'true',
    'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token, Accept'
  })
};
@Injectable({
  providedIn: 'root'  // <- ADD THIS
})
@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  private handleError: HandleError;
  heroesUrlUp = 'https://localhost:44316/api/products/UploadFile';  // URL to web api
  heroesUrl ='https://localhost:44316/api/Products'
  constructor(
    private http: HttpClient,
    httpErrorHandler: HttpErrorHandler) {
    this.handleError = httpErrorHandler.createHandleError('CategoriesService');
  }

  /** GET heroes from the server */
  getHeroes(): Observable<Product[]> {
    return this.http.get<Product[]>(this.heroesUrl +"/WithCategory")
      .pipe(
        catchError(this.handleError('getHeroes', []))
      );
  }

  upload(file): Observable<any> {

    // Create form data
    const formData = new FormData();

    // Store form name as "file" with file data
    formData.append("file", file, file.name);

    // Make http post request over api
    // with formData as req
    return this.http.post(this.heroesUrlUp, formData)
  }
  postFile(file: File): Observable<any> {
   
    console.log("uploading" + this.heroesUrl);
    // Create form data
    const formData = new FormData();
    // Store form name as "file" with file data
    formData.append("file", file, file.name);
    return this.http.post(this.heroesUrlUp, formData, { responseType: 'text' });
  }

  addHeroX(hero: Product): Observable<Product> {

    return this.http.post<Product>(this.heroesUrl, hero, httpOptions2)
      .pipe(
        catchError(this.handleError('addHero', hero))
      );
  }
  /** PUT: update the hero on the server. Returns the updated hero upon success. */
  updateHero(hero: Product): Observable<Product> {
    // httpOptions.headers =
    //   httpOptions.headers.set('Authorization', 'my-new-auth-token');
    const url = `${this.heroesUrl}/${hero.pr_id}`; // PUT api/heroes/42
    console.log("RQ:" + url);
    console.log("RX3:" + JSON.stringify(hero));
    return this.http.put<Product>(url, hero, httpOptions2)
      .pipe(
        catchError(this.handleError('updateHero', hero))
      );
  }

  /** DELETE: delete the hero from the server */
  deleteHero(id: number): Observable<{}> {
    const url = `${this.heroesUrl}/${id}`; // DELETE api/heroes/42
    return this.http.delete(url, httpOptions2)
      .pipe(
        catchError(this.handleError('deleteHero'))
      );
  }

  
}
