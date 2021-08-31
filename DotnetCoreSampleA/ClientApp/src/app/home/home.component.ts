import { Component, Inject, OnInit } from '@angular/core';
import { Product } from '../products/Product';
import { HomeService } from './home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  constructor() {
  }

  ngOnInit() {
  }
 
}
