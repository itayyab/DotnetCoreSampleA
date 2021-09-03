import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { HttpErrorHandler } from './http-error-handler.service';
import { MessageService } from './message.service';
import { CategoriesComponent } from './categories/categories.component';
import { ProductsComponent } from './products/products.component';
import { CartComponent } from './cart/cart.component';
import { ShopComponent } from './shop/shop.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastComponent } from './toast/toast.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { SharedService } from './_services/shared.service';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    CategoriesComponent,
    ProductsComponent,
    CartComponent,
    ShopComponent,
    ToastComponent,
    CheckoutComponent,
    UserComponent,
    RegistrationComponent,
    LoginComponent,
    ForbiddenComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ApiAuthorizationModule,
    NgbModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'forbidden', component: ForbiddenComponent },
      { path: 'categories', component: CategoriesComponent, canActivate: [AuthorizeGuard], data: { permittedRoles: ['ADMIN'] }},
      { path: 'products', component: ProductsComponent, canActivate: [AuthorizeGuard], data: { permittedRoles: ['ADMIN'] } },
      { path: 'cart', component: CartComponent, canActivate: [AuthorizeGuard] },
      { path: 'shop', component: ShopComponent, canActivate: [AuthorizeGuard] },
      { path: 'checkout', component: CheckoutComponent, canActivate: [AuthorizeGuard] },
     // { path: '', redirectTo: '/user/login', pathMatch: 'full' },
      {
        path: 'user', component: UserComponent,
        children: [
          { path: 'registration', component: RegistrationComponent },
          { path: 'login', component: LoginComponent }
        ]
      },
    ])
  ],
  providers: [
    HttpErrorHandler,
    MessageService,
    SharedService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
