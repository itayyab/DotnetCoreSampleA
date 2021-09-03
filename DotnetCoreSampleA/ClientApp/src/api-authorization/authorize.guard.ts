import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ApplicationPaths, QueryParameterNames } from './api-authorization.constants';
import { UserService } from '../app/_services/user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeGuard implements CanActivate {
  constructor(private router: Router, private service: UserService) {
  }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    //console.log("guard:" + JSON.stringify(next.url));
    if (localStorage.getItem('token') != null) {
      let roles = next.data['permittedRoles'] as Array<string>;
      if (roles) {
        if (this.service.roleMatch(roles)) return true;
        else {
          this.router.navigate(['/forbidden']);
          return false;
        }
      }
      return true;
    }
    else {
      //this.router.navigate(['/user/login']);
      this.handleAuthorization(false, state);
      return false;
    }

  }

  private handleAuthorization(isAuthenticated: boolean, state: RouterStateSnapshot) {
    if (!isAuthenticated) {
      this.router.navigate(['/user/login'], {
        queryParams: {
          [QueryParameterNames.ReturnUrl]: state.url
        }
      });
    }
  }
}
