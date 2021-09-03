import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NavigationStart, Router } from '@angular/router';
import { Observable, Subject, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HandleError, HttpErrorHandler } from '../http-error-handler.service';
import { Login } from '../user/login';


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

export class UserService {
  baseUrl = "";
  private handleError: HandleError;
  constructor(private router: Router,private fb: FormBuilder, private http: HttpClient, @Inject('BASE_URL') baseUrl: string, httpErrorHandler: HttpErrorHandler) {
    this.baseUrl = baseUrl;
    this.handleError = httpErrorHandler.createHandleError('UserService');
    router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        if (this.keepAfterNavigationChange) {
          // only keep for a single location change
          this.keepAfterNavigationChange = false;
        } else {
          // clear alert
          this.subject.next();
        }
      }
    });
  }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    FullName: [''],
    Passwords: this.fb.group({
      Password: ['', [Validators.required, Validators.minLength(4)]],
      ConfirmPassword: ['', Validators.required]
    }, { validator: this.comparePasswords })

  });


  private subject = new Subject<any>();
  private keepAfterNavigationChange = false;

 

  success(message: string, keepAfterNavigationChange = false) {
    this.keepAfterNavigationChange = keepAfterNavigationChange;
    this.subject.next({ type: 'success', text: message });
  }

  error(message: string, keepAfterNavigationChange = false) {
    this.keepAfterNavigationChange = keepAfterNavigationChange;
    this.subject.next({ type: 'error', text: message });
  }

  getMessage(): Observable<any> {
    return this.subject.asObservable();
  }

  comparePasswords(fb: FormGroup) {
    let confirmPswrdCtrl = fb.get('ConfirmPassword');
    //passwordMismatch
    //confirmPswrdCtrl.errors={passwordMismatch:true}
    if (confirmPswrdCtrl.errors == null || 'passwordMismatch' in confirmPswrdCtrl.errors) {
      if (fb.get('Password').value != confirmPswrdCtrl.value)
        confirmPswrdCtrl.setErrors({ passwordMismatch: true });
      else
        confirmPswrdCtrl.setErrors(null);
    }
  }

  register() {
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      FullName: this.formModel.value.FullName,
      Password: this.formModel.value.Passwords.Password
    };
    return this.http.post(this.baseUrl + 'api/ApplicationUser/Register', body, httpOptions);
  }

  login(formData: Login) {
    return this.http.post(this.baseUrl + 'api/ApplicationUser/Login', formData, httpOptions);
  }

  /*getUserProfile() {
    return this.http.get(this.baseUrl + 'api/UserProfile', { observe: 'response' });
  }*/


  /*getCountriesX(): Observable<{}> {
    *//*return this.http.get(this.baseUrl + 'api/UserProfile', { observe: 'response' }).pipe(
      catchError(this.handleError('getHeroes', Object))
    );*//*
   *//* return this.http.get(this.baseUrl + 'api/UserProfile', { observe: 'response' }).pipe(map(data => {
       return data.httpStatus
     }));
     .catch (this.handleError);*//*
  }*/
 /* getCountries(): Observable<{}> {
    return this.http.get(this.baseUrl + 'api/UserProfile', { observe: 'response' }).pipe(
      map(this.extractData),
      catchError(this.handleError('getHeroes', Object))
    );
  }*/
 /* getHeroes(): Observable<Object> {
    return this.http.get<Object>('')
      .pipe(
        catchError(this.handleError('getHeroes'))
      );
  }*/
  getHeroesXX(): Observable<any> {
    return this.http.get<boolean>(this.baseUrl + 'api/UserProfile', { observe: 'response' }).pipe(map(data => {
      //console.log("DATA:" + JSON.stringify(data));
      /*catchError(this.handleError('getHeroes'))
      {
        console.log("DATA:" + JSON.stringify(data));
      }*/
      catchError(err => {
        //
        console.log("ERRRR");
        return throwError(err);
      })
      return data;
      /*if (data.status == 200)
        return true;
      else
        return false*/
    }));
  }
  throwError(error: any) {
    console.log(error)
  }
 /* getHasAccess(): Observable<boolean> {
    this.http.get(this.baseUrl + 'api/UserProfile', { observe: 'response' }).pipe(map(data => {
      if (data.status == 200)
        return true;
      else
        return false
    }));*//*.subscribe(result => {
      //return result;
      console.log(result);
    });*//*
  }*/
      /*.map(res => {
      if (res.condition)
        return true;
      else
        return false
    })*/

  roleMatch(allowedRoles): boolean {
    var isMatch = false;
    var payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    var userRole = payLoad.role;
    allowedRoles.forEach(element => {
      if (userRole == element) {
        isMatch = true;
        return false;
      }
    });
    return isMatch;
  }
}

