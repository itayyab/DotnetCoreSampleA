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
  constructor() {
    
  }

}
