import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject, Subscription } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import { SharedService } from '../../app/_services/shared.service';
import { UserService } from '../../app/_services/user.service';

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css']
})
export class LoginMenuComponent implements OnInit, OnDestroy {
  public isAuthenticated: boolean;//Observable<Object>;
  public userName: Observable<string>;
  public userSub: Observable<string>;
  messages: any;
  subscription: Subscription;
  constructor(private userservice: UserService, private sharedService: SharedService) {
    this.subscription = this.userservice.getMessage().subscribe(message => {
      if (message) {
        this.messages = message;
        //this.messages.push(message);
      } else {
        // clear messages when empty message received
        this.messages = [];
      }
    });
  }
  
  private subject = new Subject<any>();
      
    

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
  ngOnInit() {
    this.getCountries();
    
    this.userservice.getMessage().subscribe(data => {
     // this.getCountries();
     // console.log("Data changed");
      this.setData(data);
    }
    );
  

  
    //this.subject.next();
  /*  this.sharedService.cartChange.subscribe(value => {
      this.isAuthenticated = this.userservice.getHeroesXX();
    });*/
   // this.getCountries();
  /*  this.subject.subscribe(value => {
      console.log("VAL");
      this.isAuthenticated = this.subject.asObservable();
      console.log(this.isAuthenticated.ok);
    });*/
   
    /*this.isAuthenticated.subscribe(
      this.isAuthenticated.ne
     // () => console.log('lessons loaded'),
     // console.error
    );*/
   /* this.userservice.getUserProfile().subscribe(
      res => {
        this.isAuthenticated = true;
        console.log("status code--->" + res.status);
      //  this.userDetails = res;
        console.log("getUserProfile" + JSON.stringify(res));
      },
      err => {
        this.isAuthenticated = false;
        console.log("status code--->" + err.status);
        console.log(err);
      },
    );*/

    
    //this.isAuthenticated = this.authorizeService.isAuthenticated();
   // this.userName = this.authorizeService.getUser().pipe(map(u => u && u.name));
    //console.log("userName:" + JSON.stringify(this.userName));
    // this.authorizeService.getUserSub().subscribe(heroes => {
     //  console.log("userSub:" + JSON.stringify(heroes));
      //(this.products = heroes);
      //  console.log(JSON.stringify(this.products));
    //});
   // this.onLogout();

  }
  getCountries() {
    this.userservice.getHeroesXX().subscribe(
      res => {
        //this.subject.next(res);
        //this.userservice.sendMessage(res.ok);
        this.isAuthenticated = res.ok;
        this.userservice.success("login", this.isAuthenticated);
        // this.countries = res;
        console.log(JSON.stringify(res));
        console.log("status code--->" + res);
      },
      err => {
        // this.userservice.sendMessage(false);
        this.isAuthenticated = false;
        this.userservice.error("login", false);
        console.log("status codeEEE--->" + err.status);
      })
  }
  onLogout() {
    localStorage.removeItem('token');
    this.isAuthenticated = false;
    this.userservice.error("login", false);
    //
   // this.isAuthenticated = this.userservice.getHeroesXX();
    //this.router.navigate(['/user/login']);
  }
}
