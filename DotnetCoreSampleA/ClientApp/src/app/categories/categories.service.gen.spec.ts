import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController
} from '@angular/common/http/testing';
import { HttpErrorHandler } from '../http-error-handler.service';
import { Categories } from './Categories';
import { CategoriesService } from './categories.service';
import { InjectionToken } from '@angular/core';

export const BASE_URL = new InjectionToken<string>('BASE_URL');

describe('CategoriesService', () => {
  let service: CategoriesService;

  beforeEach(() => {
    const httpErrorHandlerStub = () => ({ createHandleError: string => ({}) });
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        { provide: 'BASE_URL' , useValue: 'http://localhost/'},
        CategoriesService,
        { provide: HttpErrorHandler, useFactory: httpErrorHandlerStub }
      ]
    });
    service = TestBed.get(CategoriesService);
  });

  it('can load instance', () => {
    expect(service).toBeTruthy();
  });

  it(`heroesUrl has default value`, () => {
    expect(service.heroesUrl).toEqual(`api/categories`);
  });

  describe('addHero', () => {
    it('makes expected calls', () => {
      const httpTestingController = TestBed.get(HttpTestingController);
      const categoriesStub: Categories = <any>{};
      service.addHero(categoriesStub).subscribe(res => {
        expect(res).toEqual(categoriesStub);
      });
      const req = httpTestingController.expectOne('HTTP_ROUTE_GOES_HERE');
      expect(req.request.method).toEqual('POST');
      req.flush(categoriesStub);
      httpTestingController.verify();
    });
  });

  describe('addHeroX', () => {
    it('makes expected calls', () => {
      const httpTestingController = TestBed.get(HttpTestingController);
      const categoriesStub: Categories = <any>{};
      service.addHeroX(categoriesStub).subscribe(res => {
        expect(res).toEqual(categoriesStub);
      });
      const req = httpTestingController.expectOne('HTTP_ROUTE_GOES_HERE');
      expect(req.request.method).toEqual('POST');
      req.flush(categoriesStub);
      httpTestingController.verify();
    });
  });

  describe('updateHero', () => {
    it('makes expected calls', () => {
      const httpTestingController = TestBed.get(HttpTestingController);
      const categoriesStub: Categories = <any>{};
      service.updateHero(categoriesStub).subscribe(res => {
        expect(res).toEqual(categoriesStub);
      });
      const req = httpTestingController.expectOne('HTTP_ROUTE_GOES_HERE');
      expect(req.request.method).toEqual('PUT');
      req.flush(categoriesStub);
      httpTestingController.verify();
    });
  });

  describe('getHeroes', () => {
    let expectedHeroes: Categories[];
    beforeEach(() => {
    //  heroService = TestBed.inject(HeroesService);
      expectedHeroes = [
        { cat_id:1,cat_name:'Test Data' },
        { cat_id:2,cat_name:'Test Data' },
       ] as Categories[];
    });
    it('makes expected calls', () => {
      const httpTestingController = TestBed.get(HttpTestingController);
     // const testData: Categories []= [{cat_id:1,cat_name:'Test Data'}];
      service.getHeroes().subscribe(res => {
        expect(res).toEqual(expectedHeroes);
      });
      const req = httpTestingController.expectOne('http://localhost/api/categories');
      expect(req.request.method).toEqual('GET');
      req.flush(expectedHeroes);
      httpTestingController.verify();
    });
  });
});
