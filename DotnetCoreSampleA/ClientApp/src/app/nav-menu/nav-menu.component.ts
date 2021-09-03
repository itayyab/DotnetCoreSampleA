import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject, Subscription } from 'rxjs';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { CartService } from '../cart/cart.service';
import { SharedService } from '../_services/shared.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  isAdmin = new Subject<any>();
  public isAuthenticated: boolean;//Observable<Object>;
  public userName: Observable<string>;
  public userSub: Observable<string>;
  messages: any;
  subscription: Subscription;
  userdetails: any;


  private subject = new Subject<any>();
  constructor(private heroesService: CartService, @Inject('BASE_URL') baseUrl: string, private authorizeService: UserService, private sharedService: SharedService, private router: Router) {
   // console.log("BaeURL:" + baseUrl);
    this.subscription = this.authorizeService.getMessage().subscribe(message => {
      if (message) {
        this.messages = message;
        //this.messages.push(message);
      } else {
        // clear messages when empty message received
        this.messages = [];
      }
    });
  }

 



  ngOnDestroy() {
    // unsubscribe to ensure no memory leaks
    this.subscription.unsubscribe();
  }
  private setData(data: any) {
    //console.log("setData:" + JSON.stringify(data));
    if (!this.isAuthenticated) {
      if (data && (data.type === 'success')) {
        this.isAuthenticated = true;
      }
      else {
        this.isAuthenticated = false;
      }
    } /*else {
      if (data && (data.type === 'login') && data.success === true) {
        this.isAuthenticated = true;
      }
      else {
        this.isAuthenticated = false;
      }
    }*/
  }
  

  count: number = 0;
  ngOnInit() {
    this.getCountries();

    this.authorizeService.getMessage().subscribe(data => {
      // this.getCountries();
      // console.log("Data changed");
      this.setData(data);
     // console.log("CHKKKKK");
      this.checkisAdmin();
      this.getCount();
    }
    );
    this.getCount();
    this.sharedService.cartChange.subscribe(value => {
      this.getCount();
    });
    this.isAdmin = new BehaviorSubject(false);

    this.checkisAdmin();
  }
  getCount() {
    this.authorizeService.getHeroesXX().subscribe(heroes => {
      //console.log("userSub:" + JSON.stringify(heroes));
      this.userdetails = heroes;
      this.heroesService.getCartCount(heroes.body.userId)
        .subscribe(heroes => {
          this.count = heroes;
        });

      //(this.products = heroes);
      //  console.log(JSON.stringify(this.products));
    },
    err => {
        // this.userservice.sendMessage(false);
      this.count = 0;
        //   console.log("status codeEEE--->" + err.status);
      });
  }
  checkisAdmin() {
    if (localStorage.getItem('token') != null) {
      let roles = ['ADMIN'] as Array<string>;
      if (roles) {
        if (this.authorizeService.roleMatch(roles)) {
          this.isAdmin.next(true);
          return true;
        }
        else {
          //this.router.navigate(['/forbidden']);
          this.isAdmin.next(false);
          return false;
        }
      }
      this.isAdmin.next(false);
      return false;
    } else {
      this.isAdmin.next(false);
    }
    
  }
  

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  getCountries() {
    this.authorizeService.getHeroesXX().subscribe(
      res => {
        this.userdetails = res;
       // console.log("userSub:" + JSON.stringify(res));
        //this.subject.next(res);
        //this.userservice.sendMessage(res.ok);
        this.isAuthenticated = res.ok;
        this.authorizeService.success("login", this.isAuthenticated);
        this.heroesService.getCartCount(res.body.userId)
          .subscribe(heroes => {
            this.count = heroes;
          });
        // this.countries = res;
       // console.log(JSON.stringify(res));
       // console.log("status code--->" + res);
      },
      err => {
        // this.userservice.sendMessage(false);
        this.isAuthenticated = false;
        this.authorizeService.error("login", false);
        this.count = 0;
     //   console.log("status codeEEE--->" + err.status);
      })
  }
  onLogout() {
    localStorage.removeItem('token');
    this.isAuthenticated = false;
    this.authorizeService.error("login", false);
    this.checkisAdmin();
    this.router.navigate(['/']);
    //
    // this.isAuthenticated = this.userservice.getHeroesXX();
    //this.router.navigate(['/user/login']);
  }
}
