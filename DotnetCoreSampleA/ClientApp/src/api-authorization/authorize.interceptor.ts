import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, mergeMap, tap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeInterceptor implements HttpInterceptor {


  constructor(private router: Router) {

  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (localStorage.getItem('token') != null) {
      const clonedReq = req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + localStorage.getItem('token'))
      });
      return next.handle(clonedReq).pipe(
        tap(
          succ => {

          //  console.log("interceptor suc" + JSON.stringify(succ));
          //  console.log("interceptor suc req" + JSON.stringify(clonedReq));


          },
          err => {
          //  console.log("interceptor err" + JSON.stringify(err));
            if (err.status == 401) {
              localStorage.removeItem('token');
            //  console.log("Not authorized");
              this.router.navigateByUrl('/user/login');
            }
            else if (err.status == 403)
              this.router.navigateByUrl('/forbidden');
          }
        )
      )
    }
    else {
     /* if (req.url != 'https://localhost:44316/api/UserProfile') {
        this.router.navigateByUrl('/user/login');
      }*/
      
    //  console.log("interceptor elase" + JSON.stringify(req));
     // console.log("Not authorized");
      return next.handle(req.clone());
    }
     
  }
}
