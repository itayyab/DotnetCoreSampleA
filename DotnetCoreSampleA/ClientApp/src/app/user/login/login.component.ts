import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../_services/shared.service';
import { ToastService } from '../../_services/toast.service';
import { UserService } from '../../_services/user.service';
import { Login } from '../login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../tab.component.css']
  //styles: []
})
export class LoginComponent implements OnInit {
  formModel = {
    UserName: '',
    Password: ''
  }
  path = "";
  
  constructor(private service: UserService, private router: Router, private sharedService: SharedService, private activatedRoute: ActivatedRoute, public toastService: ToastService) { }

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      this.path = params['returnUrl'];
     // console.log("path:"+this.path);
    });
    if (localStorage.getItem('token') != null && this.path != "") {
      this.router.navigateByUrl(this.path);
    } else if (localStorage.getItem('token') != null) {
      this.router.navigateByUrl("/shop");
    }
      
  }

  onSubmit(form: Login) {
    //console.log("willredirectingto:" + this.path)
    //console.log(JSON.stringify(form));
    this.service.login(form).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        this.showSuccess('Logged in.');
        if (this.path != "") {
        //  console.log("redirectingto:" + this.path)
          this.router.navigateByUrl(this.path);
        } else {
          this.router.navigateByUrl('/shop');
        }
        
        // this.sharedService.toggleloginChange();
        this.service.success("login", true);
      },
      err => {
        if (err.status == 400) {
          //console.log('Incorrect username or password. Authentication failed.');
          this.showError('Incorrect username or password.');
        }
        // this.toastr.error('Incorrect username or password.', 'Authentication failed.');
        else {
          this.showError('Incorrect username or password.');
          console.log(err);
        }
          
      }
    );
  }
  showSuccess(message) {
    this.toastService.show(message, {
      classname: 'bg-success text-light',
      delay: 2000,
      autohide: true,
      headertext: 'Authentication successful.'
    });
  }
  showError(message) {
    this.toastService.show(message, {
      classname: 'bg-danger text-light',
      delay: 2000,
      autohide: true,
      headertext: 'Authentication failed.'
    });
  }
}
