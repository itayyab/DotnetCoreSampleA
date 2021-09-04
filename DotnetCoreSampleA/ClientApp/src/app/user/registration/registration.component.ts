import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastService } from '../../_services/toast.service';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['../tab.component.css']
 // styles: []
})
export class RegistrationComponent implements OnInit {

  constructor(public service: UserService, public toastService: ToastService, private router: Router) { }

  ngOnInit() {
    this.service.formModel.reset();
    if (localStorage.getItem('token') != null) {
      this.router.navigateByUrl("/");
    }
  }

  onSubmit() {
    this.service.register().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.service.formModel.reset();
          //console.log('New user created! Registration successful.');
          this.showSuccess('New user created!');
          //this.toastr.success('New user created!', 'Registration successful.');
        } else {
          res.errors.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
               // console.log('Username is already taken Registration failed.');
                this.showError("'Username is already taken Registration failed.");
               // this.toastr.error('Username is already taken','Registration failed.');
                break;

              default:
                //console.log('Registration failed.');
                this.showError("Failed");
             // this.toastr.error(element.description,'Registration failed.');
                break;
            }
          });
        }
      },
      err => {
        console.log(err);
      }
    );
  }
  showSuccess(message) {
    this.toastService.show(message, {
      classname: 'bg-success text-light',
      delay: 2000,
      autohide: true,
      headertext: 'Registration successful.'
    });
  }
  showError(message) {
    this.toastService.show(message, {
      classname: 'bg-danger text-light',
      delay: 2000,
      autohide: true,
      headertext: 'Registration failed.'
    });
  }

}
